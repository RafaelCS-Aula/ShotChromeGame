using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FlyerAttack : LineOfSightAttack
{
    [SerializeField] FloatVariable attackDamage;
    [SerializeField] FloatVariable attackInterval;
    [SerializeField] FloatVariable projectileSpeed;
    [SerializeField] FloatVariable destroyProjAfterSeconds;

    [SerializeField] GameEvent DamagePlayer;

    [SerializeField] GameObject projPrefab;

    
    protected override void Start() 
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack()
    {

        attackTimer = attackInterval;

        GameObject projObj = Instantiate(projPrefab);

        #region Projectile Variable Setting
        projObj.transform.position = attackOrigin.position;
        projObj.GetComponent<FlyerProjectile>().targetPos = targetH.Target.position;
        projObj.GetComponent<FlyerProjectile>().damage = attackDamage;
        projObj.GetComponent<FlyerProjectile>().projSpeed = projectileSpeed;
        projObj.GetComponent<FlyerProjectile>().destroyAfterSeconds = destroyProjAfterSeconds;
        projObj.GetComponent<FlyerProjectile>().damageEvent = DamagePlayer;
        #endregion

        anim.SetTrigger("Shoot");
    }

    

    

}
