﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaguarAttack : MonoBehaviour
{
    private Animator anim;
    private TargetHolder targetH;
    private JaguarMovement jagMov;

    [SerializeField] FloatVariable attackDamage;
    [SerializeField] FloatVariable attackInterval;
    [SerializeField] FloatVariable attackRange;


    [SerializeField] FloatVariable timeToAttack;
    [SerializeField] FloatVariable attackDuration;

    [SerializeField] GameEvent DamagePlayer;

    private bool isAttacking = false;
    private bool alreadyClose = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        targetH = GetComponent<TargetHolder>();
        jagMov = GetComponent<JaguarMovement>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (targetH.Target != null && !isAttacking)
        {
            float distToTarget = Vector3.Distance(transform.position, targetH.Target.position);
            if (distToTarget <= attackRange)
            {
                if (!alreadyClose)
                {
                    alreadyClose = true;
                    jagMov.globMoving = false;
                    anim.SetTrigger("Intermediary");
                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Intermediary")
                    || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    StartCoroutine(Attack());
                    StartCoroutine(WalkDelay());
                }
            }
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                alreadyClose = false;
                anim.SetTrigger("Run");
                jagMov.globMoving = true;
            }
        }
    }

    private IEnumerator Attack()
    {
        float delay = 0;
        float forgiveness = 1.5f;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run")) delay = 0.25f;
        print(delay);
        isAttacking = true;
        int dir = Random.Range(0, 2);
        if (dir == 0)
        {
            anim.SetTrigger("AttackLeft");
        }
        if (dir == 1)
        {
            anim.SetTrigger("AttackRight");
        }
        yield return new WaitForSeconds(timeToAttack + delay);

        float distToTarget = Vector3.Distance(transform.position, targetH.Target.position);
        if (distToTarget <= attackRange + forgiveness) DamagePlayer.RaiseDamageArg(attackDamage);

    }
    private IEnumerator WalkDelay()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;

    }
}
