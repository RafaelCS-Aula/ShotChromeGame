using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using UnityEngine.AI;
using UnityEditor;

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
    [Foldout("Events")]
    [SerializeField] private UnityEvent OnHealed;

    [Foldout("Positional Events")]
    [SerializeField] private UnityEvent<Vector3> OnShotgunDamaged;
    [Foldout("Positional Events")]
    [SerializeField] private UnityEvent<Vector3> OnAoEDamaged;




    [SerializeField] FloatVariable MaxHealth;
    [SerializeField] FloatVariable OverHealAmount;
    [SerializeField] FloatVariable ThunderPowerGift;
    [SerializeField] FloatData currentThunderPower;

    [SerializeField] private bool blockHealing;

    private bool _usesWaypointMovement;
    [SerializeField] private bool usesNavmesh;

    private float _health;
    private MonoBehaviour _lastDamageSource = null;
    private bool _died = false;

    private Collider _col;

    public bool isBuffed = false;
    private void Start()
    {
        _health = MaxHealth;
        _col = GetComponent<Collider>();
        _usesWaypointMovement = GetComponent<WaypointMovement>();
    }
    private void Update()
    {
        if (_health <= 0 && !_died) EnemyDeath();
        if(_health > MaxHealth + OverHealAmount)
        {
            _health = MaxHealth + OverHealAmount;
        }
    }

    public void OnDamaged(float damage, MonoBehaviour source = null)
    {
        //print("Damage: " + damage);
        _health -= damage;
        _lastDamageSource = source;
        Vector3 sourceDir = Vector3.zero;

        if (source == null)
            OnDefaultDamaged.Invoke();
        else
            sourceDir = transform.position - source.gameObject.transform.position;

        if (source is Shotgun)
        {
            OnShotgunDamaged.Invoke(sourceDir);
        }
        else if (source is AreaofEffect)
        {
            OnAoEDamaged.Invoke(sourceDir);
        }
    }

    public void ReceiveHeal(float value)
    {
        if(blockHealing)
            return;
        _health += value;
    }

    public void EnemyDeath()
    {
        _died = true;

        if (_usesWaypointMovement)
        {
            GetComponent<WaypointMovement>().GetCurrentWaypoint().ToggleOccupation();
            GetComponent<WaypointMovement>().SetMoving(false);

        }
        else if (usesNavmesh)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }

        if (_lastDamageSource is Shotgun)
        {
            currentThunderPower.ApplyChange(ThunderPowerGift);
            OnShotgunKill.Invoke();
        }
        else if (_lastDamageSource is AreaofEffect)
        {
            OnAoEKill.Invoke();
        }
        else
        {
            OnDefaultKill.Invoke();
        }

        StartCoroutine(DestroyInSecs(0.4f));
    }

    private IEnumerator DestroyInSecs(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() 
    {
        
        Handles.Label(transform.position + Vector3.up * 2, $"{_health}/{MaxHealth.Value}");
    }
#endif
}
