using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] FloatVariable MaxHealth;
    private float health = 100;

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
        if (health <= 0) Destroy(gameObject);
    }

    public void OnDamaged(float damage)
    {
        //print("Damage: " + damage);
        health -= damage;
        time = 0;
        changingToDamaged = true;
        changingToOriginal = false;
        StartCoroutine("ColorChange");
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
