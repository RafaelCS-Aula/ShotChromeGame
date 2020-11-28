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
public class Vector3Variable : GameVariable<Vector3Data, Vector3>
{
    public static implicit operator Vector3(Vector3Variable reference)
    {
        return reference.Value;
    }
}
