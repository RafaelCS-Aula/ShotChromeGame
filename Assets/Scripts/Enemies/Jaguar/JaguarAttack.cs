using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaguarAttack : MonoBehaviour
{
    private Animator anim;
    private TargetHolder targetH;
    private JaguarMovement jagMov;

    [SerializeField] FloatVariable attackDamage;
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
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run")) print("Run");
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("PunchIdle")) print("PunchIdle");
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("PunchLeft")) print("PunchLeft");
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("PunchRight")) print("PunchRight");


        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run")) jagMov.globMoving = false;



        if (targetH.Target != null && !isAttacking)
        {
            float distToTarget = Vector3.Distance(transform.position, targetH.Target.position);
            if (distToTarget <= attackRange)
            {
                if (!alreadyClose)
                {
                    alreadyClose = true;
                    anim.SetTrigger("PunchIdle");
                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("PunchIdle")
                    || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    StartCoroutine(Attack());
                    StartCoroutine(RunDelay());
                }
            }
            else
            {
                alreadyClose = false;
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    jagMov.globMoving = true;
                    anim.SetTrigger("Run");
                }
            }
        }
    }

    private IEnumerator Attack()
    {
        float delay = 0;
        float forgiveness = 1.5f;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run")) delay = 0.05f;

        isAttacking = true;
        int dir = Random.Range(0, 2);
        if (dir == 0)
        {
            anim.SetTrigger("PunchLeft");
            print("PunchLeft");
        }
        if (dir == 1)
        {
            anim.SetTrigger("PunchRight");
            print("PunchRight");
        }
        yield return new WaitForSeconds(timeToAttack + delay);

        float distToTarget = Vector3.Distance(transform.position, targetH.Target.position);
        if (distToTarget <= attackRange + forgiveness) DamagePlayer.RaiseDamageArg(attackDamage);

    }
    private IEnumerator RunDelay()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;

    }
}
