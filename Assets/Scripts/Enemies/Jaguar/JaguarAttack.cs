using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class JaguarAttack : MonoBehaviour
{
    private Animator anim;
    private TargetHolder targetH;
    private JaguarMovement jagMov;
    private EnemyHealth eH;

    public UnityEvent OnAttack;

    [SerializeField] FloatVariable attackDamage;
    [SerializeField] FloatVariable attackRange;


    [SerializeField] FloatVariable timeToAttack;
    [SerializeField] FloatVariable attackDuration;


    [SerializeField] FloatVariable shotgunHitDuration;
    [SerializeField] FloatVariable lightningHitDuration;

    [SerializeField] GameEvent DamagePlayer;

    private bool isAttacking = false;
    private bool alreadyClose = false;

    private bool isHit = false;
    private bool running = true;
    private bool idlePunching = false;

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

        if (targetH.Target != null && !isAttacking && !isHit)
        {
            float distToTarget = Vector3.Distance(transform.position, targetH.Target.position);
            if (distToTarget <= attackRange)
            {
                if (!alreadyClose)
                {
                    alreadyClose = true;
                    running = false;
                    anim.SetTrigger("PunchIdle");
                    idlePunching = true;
                }
                if (idlePunching || running)
                {
                    running = false;
                    idlePunching = false;
                    StartCoroutine(Attack());
                    StartCoroutine(RunDelay());
                }
            }
            else
            {
                alreadyClose = false;
                if (!running)
                {
                    anim.SetTrigger("Run");
                    jagMov.globMoving = true;
                    running = true;
                }
            }
        }

        if (eH.Hit() && !isHit)
        {
            isHit = true;
            jagMov.globMoving = false;
            running = false;
            anim.SetTrigger("Hit");

            StartCoroutine(HitStop());
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
            if (!isHit)
            {
                jagMov.globMoving = false;
                anim.SetTrigger("PunchLeft");
            }
        }
        if (dir == 1)
        {
            if (!isHit)
            {
                jagMov.globMoving = false;
                anim.SetTrigger("PunchRight");
            }
        }
        OnAttack.Invoke();
        yield return new WaitForSeconds(timeToAttack + delay);

        float distToTarget = Vector3.Distance(transform.position, targetH.Target.position);
        if (distToTarget <= attackRange + forgiveness) DamagePlayer.RaiseDamageArg(attackDamage);


    }
    private IEnumerator RunDelay()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        alreadyClose = false;
    }

    private IEnumerator HitStop()
    {
        float delay = 10;

        MonoBehaviour source = eH.GetLastDamageSource();

        if (source is Shotgun) delay = shotgunHitDuration.Value;
        else delay = lightningHitDuration.Value;

        yield return new WaitForSeconds(delay);
        jagMov.globMoving = true;
        anim.SetTrigger("Run");
        running = true;
        eH.SetHit(false);
        isHit = false;
    }
}
