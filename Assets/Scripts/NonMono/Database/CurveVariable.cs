// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

//-------------------------
// Altered by Rafael Castro e Silva
// 2020
//-----------------------------

using System;
using UnityEngine;

[Serializable]
public class CurveVariable : GameVariable<CurveData, AnimationCurve>
{
    public static implicit operator AnimationCurve(CurveVariable reference)
    {
        return reference.Value;
    }
}
