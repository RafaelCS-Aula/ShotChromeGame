using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAttack : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] FloatVariable flyingHeight;

    [SerializeField] FloatVariable attackDamage;
    [SerializeField] FloatVariable attackInterval;
    [SerializeField] FloatVariable attackRange;
    [SerializeField] FloatVariable projectileSpeed;

    [SerializeField] GameEvent DamagePlayer;

    [SerializeField] Transform projOrigin;
    [SerializeField] GameObject projPrefab;

    private float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0) attackTimer -= Time.deltaTime;

        Vector3 targetSameHeight = new Vector3(target.position.x, flyingHeight, target.position.z);
        float distToTarget = Vector3.Distance(targetSameHeight, transform.position);

        if (distToTarget <= attackRange && attackTimer <= 0) Attack();

    }

    private void Attack()
    {
        attackTimer = attackInterval;

        GameObject projObj = Instantiate(projPrefab);

        projObj.transform.position = projOrigin.position;
        projObj.GetComponent<FlyerProjectile>().targetPos = target.position;
        projObj.GetComponent<FlyerProjectile>().damage = attackDamage;
        projObj.GetComponent<FlyerProjectile>().projSpeed = projectileSpeed;
        projObj.GetComponent<FlyerProjectile>().damageEvent = DamagePlayer;
    }
    #region  Kamikaze Attack
    /*
    private bool isAttacking;

    private float collisionDistance = 3;

    private Vector3 vel;

    private Vector3 attackOrigin;
    // Start is called before the first frame update
    void Start()
    {
        vel = Vector3.zero;
        attackOrigin = Vector3.zero;
        attackTimer = 0;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0) attackTimer -= Time.deltaTime;

        float distToTarget = Vector3.Distance(target.position, transform.position);

        Vector3 targetSameHeight = new Vector3(target.position.x, transform.position.y, target.position.z);

        float distToTargetHeight = Vector3.Distance(targetSameHeight, transform.position);

        if (attackTimer <= 0)
        {
            if (distToTargetHeight <= attackRange && distToTarget > collisionDistance)
            {
                if (!isAttacking) attackOrigin = transform.position;
                isAttacking = true;
                MoveToTarget();
            }
        }

        if (distToTarget <= collisionDistance && isAttacking) Attack();

        if (distToTarget <= attackRange && transform.position.y < flyingHeight && !isAttacking) ReturnToHeight();
        *
}



private void MoveToTarget()
    {
        //print("MOVETOTARGET");
        Vector3 wantedVel = target.position - transform.position;
        wantedVel = wantedVel.normalized * attackSpeed;

        Vector3 steering = wantedVel - vel;

        vel = Vector3.ClampMagnitude(vel + steering, attackSpeed);
        transform.position += new Vector3(vel.x, vel.y, vel.z) * Time.deltaTime;

        if (vel != Vector3.zero) transform.forward = vel.normalized;
    }

    private void ReturnToHeight()
    {
        print("RETURNTOHEIGHT");
        Vector3 wantedVel = attackOrigin - transform.position;
        wantedVel = wantedVel.normalized * attackSpeed;

        Vector3 steering = wantedVel - vel;

        vel = Vector3.ClampMagnitude(vel + steering, attackSpeed);
        transform.position += new Vector3(vel.x, 0, vel.z) * Time.deltaTime;

        if (vel != Vector3.zero) transform.forward = vel.normalized;

        if (transform.position == attackOrigin)
        {
            attackTimer = attackInterval;
        }
    }

    private void Attack()
    {
        print("ATTACK");
        attackTimer = attackInterval;
        DamagePlayer.RaiseDamageArg(attackDamage.Value);
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackOrigin, 1);
    }
    */
    #endregion
}
