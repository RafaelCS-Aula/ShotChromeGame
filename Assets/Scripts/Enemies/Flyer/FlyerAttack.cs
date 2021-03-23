using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FlyerAttack : MonoBehaviour
{
    private Animator anim;
    private TargetHolder targetH;

    [SerializeField] FloatVariable attackDamage;
    [SerializeField] FloatVariable attackInterval;
    [SerializeField] FloatVariable projectileSpeed;
    [SerializeField] FloatVariable destroyProjAfterSeconds;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] GameEvent DamagePlayer;

    [SerializeField] Transform projOrigin;
    [SerializeField] GameObject projPrefab;

    [SerializeField] private BoolVariable shootWhileMoving;

    private float attackTimer;
    private bool attackLocked;

    private Vector3 currentPos = Vector3.zero;
    private Vector3 lastPos = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        targetH = GetComponent<TargetHolder>();
        StartCoroutine(LockAttackForSeconds(2));
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackLocked)
        {
            if (attackTimer > 0) attackTimer -= Time.deltaTime;

            float distToTarget = Vector3.Distance(targetH.Target.position, transform.position);

            if (attackTimer <= 0 && CheckForVisual())
            {
                if (shootWhileMoving) Attack();
                else if (!CheckMovement()) Attack();
            }
        }
    }

    private void Attack()
    {

        attackTimer = attackInterval;

        GameObject projObj = Instantiate(projPrefab);

        #region Projectile Variable Setting
        projObj.transform.position = projOrigin.position;
        projObj.GetComponent<FlyerProjectile>().targetPos = targetH.Target.position;
        projObj.GetComponent<FlyerProjectile>().damage = attackDamage;
        projObj.GetComponent<FlyerProjectile>().projSpeed = projectileSpeed;
        projObj.GetComponent<FlyerProjectile>().destroyAfterSeconds = destroyProjAfterSeconds;
        projObj.GetComponent<FlyerProjectile>().damageEvent = DamagePlayer;
        #endregion

        anim.SetTrigger("Shoot");
    }

    private bool CheckMovement()
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

    private bool CheckForVisual()
    {
        Vector3 originP = projOrigin.position;

        Ray rayshow = new Ray(originP, targetH.Target.position - originP);
        RaycastHit hitinfo;

        if (Physics.Raycast(rayshow, out hitinfo, 3000))
        {
            if (hitinfo.collider != null)
            {
                if (playerLayer == (playerLayer | (1 << hitinfo.collider.gameObject.layer))) return true;
                else return false;
            }
        }
        return false;
    }
}
