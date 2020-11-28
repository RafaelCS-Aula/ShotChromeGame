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
public class StringVariable : GameVariable<StringData, string>
{
    public static implicit operator string(StringVariable reference)
    {
        return reference.Value;
    }
}
