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
public class FloatVariable : GameVariable<FloatData, float>
{
    public static implicit operator float(FloatVariable reference)
    {
        return reference.Value;
    }

    public static implicit operator int(FloatVariable reference)
    {
        return (int)Math.Round(reference.Value);
    }
}
