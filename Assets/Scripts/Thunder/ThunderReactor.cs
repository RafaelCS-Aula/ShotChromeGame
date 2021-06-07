using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThunderReactor : MonoBehaviour
{
    public delegate void NoParam();
    public delegate void VectorParam(Vector3 point);

    public NoParam OnHitByThunder = delegate(){};
    public VectorParam OnSpotHitByThunder = delegate(Vector3 point){};

}
