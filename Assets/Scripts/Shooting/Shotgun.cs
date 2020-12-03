using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] IntVariable pelletsPerShot;

    [SerializeField] FloatVariable shotConeAngle;
    [SerializeField] FloatVariable defaultFireRate;
    [SerializeField] FloatVariable scfireRateModifier;

    [SerializeField] KeycodeVariable shootInput;

    [SerializeField] Transform shotOrigin;

    private List<Quaternion> pellets;

    private float shotInterval;
    private float shotTimer;

    public bool isSuperCharged;

    private void Awake()
    {
        pellets = new List<Quaternion>(new Quaternion[pelletsPerShot]);
        shotTimer = 0;
    }

    private void Update()
    {
        shotInterval = GetShotInterval();

        shotTimer -= Time.deltaTime;

        if (Input.GetKeyDown(shootInput) && shotTimer <= 0) Shoot();

        print(shotTimer);
    }

    private void Shoot()
    {
        for (int i = 0; i < pellets.Count; i++)
        {
            pellets[i] = Random.rotation;
            Quaternion rotation = Quaternion.RotateTowards(shotOrigin.rotation, pellets[i], shotConeAngle);

            Debug.DrawRay(shotOrigin.position, rotation * Vector3.forward, Color.red, 0.1f);
        }

        shotTimer = shotInterval;
    }

    private float GetShotInterval()
    {
        float currentFirerate = isSuperCharged ? defaultFireRate * scfireRateModifier : defaultFireRate;
        return 1 / currentFirerate;
    }
}
