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
public class KeycodeVariable : GameVariable<KeycodeData, KeyCode>
{
    public static implicit operator KeyCode(KeycodeVariable reference)
    {
        return reference.Value;
    }
}
