using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaguarAttack : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] FloatVariable attackDamage;
    [SerializeField] FloatVariable attackInterval;
    [SerializeField] FloatVariable attackRange;

    [SerializeField] GameEvent DamagePlayer;

    private float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (attackTimer > 0) attackTimer -= Time.deltaTime;
            float distToTarget = Vector3.Distance(transform.position, target.position);
            if (distToTarget <= attackRange && attackTimer <= 0) Attack();
        }
    }

    private void Attack()
    {
        attackTimer = attackInterval;
        DamagePlayer.RaiseDamageArg(attackDamage);
    }
}
