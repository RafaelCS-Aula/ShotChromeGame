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

[Serializable]
public class BoolVariable : GameVariable<BoolData, bool>
{
    public static implicit operator bool(BoolVariable reference)
    {
        return reference.Value;
    }
}
