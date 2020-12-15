using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Shotgun : InputReceiverBase
{
    #region InspectorVars
    [SerializeField] IntVariable pelletsPerShot;

    [SerializeField] FloatVariable maxPelletDamage;
    [SerializeField] FloatVariable maxShotDistance;
    [SerializeField] FloatVariable shotConeAngle;
    [SerializeField] FloatVariable defaultFireRate;
    [SerializeField] FloatVariable scfireRateModifier;

    [SerializeField] BoolVariable isSuperCharged;

    [SerializeField] CurveVariable pelletDamageFalloff;

    [SerializeField] private LayerMask layersToHit;
    #endregion

    private List<Quaternion> pellets;

    private float shotInterval;
    private float shotTimer;

    [SerializeField] private LayerMask enemyLayer;


    protected override void RegisterForInput()
    {
        base.RegisterForInput();
        if(UseInput)
            InputHolder.InpShoot += InputDown;
        else if(!UseInput)
            InputHolder.InpShoot -= InputDown;
    }

    private void InputDown(bool key) => _input = key;
    private void Awake()
    {
        pellets = new List<Quaternion>(new Quaternion[pelletsPerShot]);
        shotTimer = 0;
    }

    private void Update()
    {
        shotInterval = GetShotInterval();

        if (_input && shotTimer > 0) shotTimer -= Time.deltaTime;

    }

    private void TryToShoot()
    {
        if(shotTimer <= 0) 
            Shoot();
    }

    [Button("Test Shoot")]
    private void Shoot()
    {
        int numberOfHits = 0;

        for (int i = 0; i < pellets.Count; i++)
        {
            RaycastHit hitInfo;

            Quaternion originR = Camera.main.transform.rotation;
            Vector3 originP = Camera.main.transform.position;
            originP = new Vector3(originP.x, originP.y, originP.z + Camera.main.nearClipPlane);

            pellets[i] = Random.rotation;
            Quaternion rotation = Quaternion.RotateTowards(originR, pellets[i], shotConeAngle);

            Ray pelletRay = new Ray(originP, rotation * Vector3.forward);

            Debug.DrawRay(originP, rotation * Vector3.forward.normalized *50, Color.red, 0.1f);

            if (Physics.Raycast(pelletRay, out hitInfo, Mathf.Infinity, layersToHit))
            {
                if (hitInfo.transform.gameObject.layer == enemyLayer)
                {
                    numberOfHits++;

                    //Calculate Damage Using Curves
                    float hitDist = (originP - hitInfo.transform.position).magnitude;
                    float hitEffect = hitDist / maxShotDistance;
                    float hitValue = pelletDamageFalloff.Value.Evaluate(hitEffect);
                    float dealtDamage = hitValue * maxPelletDamage; // USE WHEN DEALING DAMAGE TO ENEMY
                    //print("DAMAGE: " + dealtDamage);

                    EnemyHealth enemyHealth = hitInfo.transform.gameObject.GetComponent<EnemyHealth>();

                    if (enemyHealth == null) enemyHealth = hitInfo.transform.gameObject.GetComponentInParent<EnemyHealth>();

                    enemyHealth.OnDamaged(dealtDamage);
                }

                #region Damage Using Formula
                /*
                float damageDropPU = maxPelletDamage / maxShotDistance;      
                float damageLoss = hitDist * damageDropPU;
                float dealtDamageF = maxPelletDamage - damageLoss;
                if (dealtDamageF < 0.01f) dealtDamageF = 0;
                */
                #endregion
            }
        }
        print("HITS: " + numberOfHits);
        shotTimer = shotInterval;
    }

    private float GetShotInterval()
    {
        float currentFirerate = isSuperCharged ? defaultFireRate * scfireRateModifier : defaultFireRate;
        return 1 / currentFirerate;
    }
}
