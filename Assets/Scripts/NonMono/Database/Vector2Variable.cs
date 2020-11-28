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
public class Vector2Variable : GameVariable<Vector2Data, Vector2>
{
    public static implicit operator Vector2(Vector2Variable reference)
    {
        return reference.Value;
    }
}
