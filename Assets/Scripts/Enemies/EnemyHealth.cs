using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using UnityEngine.AI;

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

    private bool _usesWaypointMovement;
    [SerializeField] private bool usesNavmesh;

    private float health = 100;
    private MonoBehaviour lastDamageSource = null;
    private bool died = false;

    private Collider col;

    private void Start()
    {
        health = MaxHealth;
        col = GetComponent<Collider>();
        _usesWaypointMovement = GetComponent<WaypointMovement>();
    }
    private void Update()
    {
        if (health <= 0 && !died) EnemyDeath();
    }

    public void OnDamaged(float damage, MonoBehaviour source = null)
    {
        //print("Damage: " + damage);
        health -= damage;
        lastDamageSource = source;
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

    public void EnemyDeath()
    {
        died = true;

        if (_usesWaypointMovement)
        {
            GetComponent<WaypointMovement>().GetCurrentWaypoint().ToggleOccupation();
            GetComponent<WaypointMovement>().SetMoving(false);

        }
        else if (usesNavmesh)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }

        if (lastDamageSource is Shotgun)
        {
            currentThunderPower.ApplyChange(ThunderPowerGift);
            OnShotgunKill.Invoke();
        }
        else if (lastDamageSource is AreaofEffect)
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
}
