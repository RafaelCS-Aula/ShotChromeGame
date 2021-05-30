using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaguarAttack : MonoBehaviour
{
    private Animator anim;
    private TargetHolder targetH;
    private JaguarMovement jagMov;
    private EnemyHealth eH;

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
        eH = GetComponent<EnemyHealth>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run")) jagMov.globMoving = false;
        else jagMov.globMoving = true;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) jagMov.globMoving = false;

        print(jagMov.globMoving);

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
