using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetHolder))]
public abstract class LineOfSightAttack : MonoBehaviour
{
    protected Animator anim;
    protected TargetHolder targetH;

    protected float attackTimer;
    protected bool attackLocked;

    protected Vector3 currentPos = Vector3.zero;
    protected Vector3 lastPos = Vector3.zero;

    [SerializeField] protected BoolVariable allowAttackWhileMoving;
    [SerializeField] protected Transform attackOrigin;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        targetH = GetComponent<TargetHolder>();
        StartCoroutine(LockAttackForSeconds(2));
    }

    protected virtual void Update()
    {
        if (!attackLocked)
        {
            if (attackTimer > 0) attackTimer -= Time.deltaTime;

            if (attackTimer <= 0 && targetH.HasLineOfSightToTarget(attackOrigin))
            {
                if (allowAttackWhileMoving) Attack();
                else if (!CheckMovement()) Attack();
            }
        }
    }

    protected abstract void Attack();

    protected bool CheckMovement()
    {
        bool moved;

        currentPos = transform.position;

        if (currentPos == lastPos)
        {
            moved = false;
        }
        else moved = true;

        lastPos = currentPos;

        return moved;
    }

    public IEnumerator LockAttackForSeconds(int time)
    {
        attackLocked = true;
        yield return new WaitForSeconds(time);
        attackLocked = false;
    }
    
}
