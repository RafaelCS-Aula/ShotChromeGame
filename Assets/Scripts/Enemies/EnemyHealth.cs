using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class EnemyHealth : MonoBehaviour
{

    [Foldout("Events")]
    [SerializeField] private UnityEvent OnShotgunKill;
    [Foldout("Events")]
    [SerializeField] private UnityEvent OnAoEKill;
    [Foldout("Events")]
    [SerializeField] private UnityEvent OnDefaultKill;
    [Foldout("Events")]
    [SerializeField] private UnityEvent OnDefaultDamaged;

    [Foldout("Positional Events")]
    [SerializeField] private UnityEvent<Vector3> OnShotgunDamaged;
    [Foldout("Positional Events")]
    [SerializeField] private UnityEvent<Vector3> OnAoEDamaged;
    
    


    [SerializeField] FloatVariable MaxHealth;
    [SerializeField] FloatVariable ThunderPowerGift;
    [SerializeField] FloatData currentThunderPower;
    private float health = 100;
    private MonoBehaviour lastDamageSource = null;

    #region ColorChange
    private Renderer renderer;

    bool changingToOriginal = false;
    bool changingToDamaged = false;

    private float duration = 0.1f;
    private float time = 0;

    private Color originalColor;
    [SerializeField] private Color damageColor;
    #endregion

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        health = MaxHealth;
    }
    private void Update()
    {
        if (health <= 0) EnemyDeath();
    }

    public void OnDamaged(float damage, MonoBehaviour source = null)
    {
        //print("Damage: " + damage);
        health -= damage;
        lastDamageSource = source;
        Vector3 sourceDir = Vector3.zero; 

        if(source == null)
            OnDefaultDamaged.Invoke();
        else
            sourceDir = transform.position - source.gameObject.transform.position;

        if(source is Shotgun)
        {
            OnShotgunDamaged.Invoke(sourceDir);
            

        }
        else if(source is AreaofEffect)
        {
            OnAoEDamaged.Invoke(sourceDir);
        }

        time = 0;
        changingToDamaged = true;
        changingToOriginal = false;
        StartCoroutine("ColorChange");
    }

    public void EnemyDeath()
    {
        if(lastDamageSource is Shotgun)
        {
            currentThunderPower.ApplyChange(ThunderPowerGift);
            OnShotgunKill.Invoke();
        }
        else if(lastDamageSource is AreaofEffect)
        {
            OnAoEKill.Invoke();
        }
        else
            OnDefaultKill.Invoke();
        Destroy(gameObject);
    }

    public IEnumerator ColorChange()
    {
        float ElapsedTime = 0.0f;
        float TotalTime = 0.1f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            renderer.material.color = Color.Lerp(originalColor, damageColor, (ElapsedTime / TotalTime));
            yield return null;
        }

        ElapsedTime = 0.0f;
        TotalTime = 0.1f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            renderer.material.color = Color.Lerp(damageColor, originalColor, (ElapsedTime / TotalTime));
            yield return null;
        }
    }
}
