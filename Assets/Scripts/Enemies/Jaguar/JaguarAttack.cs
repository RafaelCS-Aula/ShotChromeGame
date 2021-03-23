﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaguarAttack : MonoBehaviour
{
    private TargetHolder targetH;

    [SerializeField] FloatVariable attackDamage;
    [SerializeField] FloatVariable attackInterval;
    [SerializeField] FloatVariable attackRange;

    [SerializeField] GameEvent DamagePlayer;

    private float attackTimer;
    // Start is called before the first frame update
    private void Start()
    {
        attackTimer = 0;
        targetH = GetComponent<TargetHolder>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (targetH.Target != null)
        {
            if (attackTimer > 0) attackTimer -= Time.deltaTime;
            float distToTarget = Vector3.Distance(transform.position, targetH.Target.position);
            if (distToTarget <= attackRange && attackTimer <= 0) Attack();
        }
    }

    private void Attack()
    {
        attackTimer = attackInterval;
        DamagePlayer.RaiseDamageArg(attackDamage);
    }
}