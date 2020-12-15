using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class ThunderPower : MonoBehaviour
{
    [SerializeField] private FloatVariable maxPower;
    [SerializeField] private FloatVariable currentPower;
    [SerializeField] private FloatData currentRadius;
    [SerializeField] private FloatData currentDamage;
    [SerializeField] private IntData currentAmmo;

    [Label("Read Keys Directly (Radius Curve)")]
    [SerializeField] private bool readRadiusKeysDirectly;
    [SerializeField] private CurveVariable radiusCurve;

    [Label("Read Keys Directly (Ammo Curve)")]
    [SerializeField] private bool readAmmoKeysDirectly;
    [SerializeField] private CurveVariable ammoCurve;

    [Label("Read Keys Directly (Damage Curve)")]
    [SerializeField] private bool readDamageKeysDirectly;
    [SerializeField] private CurveVariable damageCurve;

    [SerializeField] private CurveVariable ReplenishmentCurve;

    [SerializeField] private bool consumeOnUse;
    private void Update() 
    {

        currentPower.Variable?.ApplyChange(
            ReplenishmentCurve.Value.Evaluate(
                (currentPower / maxPower)) * Time.deltaTime);

        if(currentPower > maxPower)
        {
            currentPower = maxPower;
        }

        /*print(ReplenishmentCurve.Value.Evaluate(
                (currentPower / maxPower)) * Time.deltaTime);*/

    }
    public void EvaluatePower()
    {
        float rad = 0;
        float amm = 0;
        float dmg = 0;
        float currTime = currentPower / maxPower;

        if(!readRadiusKeysDirectly)
            rad = radiusCurve.Value.Evaluate(currTime);
        else
            rad = GetNearestLowerKey(radiusCurve, currTime);

        if(!readAmmoKeysDirectly)
            amm = ammoCurve.Value.Evaluate(currTime);
        else
            amm = GetNearestLowerKey(ammoCurve, currTime);

        if(!readDamageKeysDirectly)
            dmg = damageCurve.Value.Evaluate(currTime);
        else
            dmg = GetNearestLowerKey(damageCurve, currTime);
        
        currentAmmo.SetValue((int)amm);
        currentRadius.SetValue(rad);
        currentDamage.SetValue(dmg);

        //print("From Power: Radius - " + rad );

        if(consumeOnUse)
            currentPower.Variable.SetValue(0);

    }

    private float GetNearestLowerKey(AnimationCurve curve, float time)
    {
        Keyframe[] keys = curve.keys;

        for (int i = keys.Length - 1; i > 0; i--)
        {
            if(keys[i].time <= time)
            {
                return keys[i].value;
            }
        }

        return 0;
    }

}
