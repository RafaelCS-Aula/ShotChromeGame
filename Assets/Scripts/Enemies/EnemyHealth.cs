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

    private void Start()
    {
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
}
