using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Shotgun : InputReceiverBase
{
    #region InspectorVars

    [Foldout("Events")]
    [SerializeField] private UnityEvent OnShootEvent;
    [Foldout("Events")]
    [SerializeField] private UnityEvent OnChargedShootEvent;

    [SerializeField] IntVariable pelletsPerShot;

    [SerializeField] FloatVariable maxPelletDamage;
    [SerializeField] FloatVariable maxShotDistance;
    [SerializeField] FloatVariable shotConeAngle;
    [SerializeField] FloatVariable defaultFireRate;
    [SerializeField] FloatVariable scfireRateModifier;

    [SerializeField] BoolData isSuperCharged;

    [SerializeField] CurveVariable pelletDamageFalloff;

    [SerializeField] private Camera cameraShotOrigin;

    [SerializeField] private LayerMask layersToHit;

    [SerializeField] private FloatVariable currentAmmo;
    [SerializeField] private FloatVariable maxAmmo;
    //[SerializeField] private IntVariable chargedAmmo;
    #endregion

    private List<Quaternion> pellets;

    private float shotInterval;
    private float shotTimer;

    [SerializeField] private LayerMask enemyLayer;

    private void OnEnable()
    {
        RegisterForInput();
    }

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
        currentAmmo.OverrideValue(maxAmmo);

        pellets = new List<Quaternion>(new Quaternion[pelletsPerShot]);

        shotTimer = 0;
    }

    private void Update()
    {
        
        shotInterval = GetShotInterval();

        if (shotTimer > 0) shotTimer -= Time.deltaTime;

        if(currentAmmo > maxAmmo)
            currentAmmo.OverrideValue(maxAmmo.Value);

        if (_input) TryToShoot();
    }

    private void TryToShoot()
    {
        if (shotTimer <= 0 && currentAmmo > 0)
        {
            Shoot();
        }
            
    }

    [Button("Test Shoot")]
    private void Shoot()
    {
        
        currentAmmo.OverrideValue(currentAmmo - 1);
        // Consume Ammo
        if(isSuperCharged)
        {

            //chargedAmmo.OverrideValue(chargedAmmo -  1);
            OnChargedShootEvent.Invoke();
        }  
        else
        {

            OnShootEvent.Invoke();
        }
            

        int numberOfHits = 0;

        for (int i = 0; i < pellets.Count; i++)
        {
            RaycastHit hitInfo;
            
            Quaternion originR = cameraShotOrigin.transform.rotation;
            Vector3 originP = cameraShotOrigin.transform.position;
            originP = new Vector3(originP.x, originP.y, originP.z + cameraShotOrigin.nearClipPlane);

            pellets[i] = Random.rotation;
            Quaternion rotation = Quaternion.RotateTowards(originR, pellets[i], shotConeAngle);

            Ray pelletRay = new Ray(originP, rotation * Vector3.forward);

            Debug.DrawRay(originP, rotation * Vector3.forward.normalized *50, Color.red, 0.1f);

            if (Physics.Raycast(pelletRay, out hitInfo, Mathf.Infinity, layersToHit))
            {
                if ((enemyLayer | (1 << hitInfo.transform.gameObject.layer)) == enemyLayer)
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

                    if(enemyHealth != null)
                        enemyHealth.OnDamaged(dealtDamage, this);
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
        //print("HITS: " + numberOfHits);
        shotTimer = shotInterval;
    }

    private float GetShotInterval()
    {
        float currentFirerate = isSuperCharged ? defaultFireRate * scfireRateModifier : defaultFireRate;
        return 1 / currentFirerate;
    }

   /* void IResourceHolder<AmmoResource>.ReceiveResource(float amount)
    {
        currentAmmo.OverrideValue(currentAmmo.Value + (int)amount);
        if(currentAmmo.Value > maxAmmo)
            currentAmmo.OverrideValue(maxAmmo);
    }*/
   /* void IResourceHolder<ThunderAmmoResource>.ReceiveResource(float amount)
    {
        chargedAmmo.OverrideValue(chargedAmmo.Value + (int)amount);
    }*/
    
}
