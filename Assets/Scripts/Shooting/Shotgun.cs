using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] IntVariable pelletsPerShot;

    [SerializeField] FloatVariable shotConeAngle;
    [SerializeField] FloatVariable defaultFireRate;
    [SerializeField] FloatVariable scfireRateModifier;
}
