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
public abstract class GameVariable<T, K> where T:  DatabaseVariable<K>
{
    public bool UseInspectorValue = true;
    public K InspectorValue;
    public T Variable;

    public GameVariable()
    { }

    public GameVariable(K value)
    {
        UseInspectorValue = true;
        InspectorValue = value;
    }

    public K Value
    {
        get { return UseInspectorValue || Variable == null ? InspectorValue : Variable.Value; }
    }

   /* public static implicit operator float(Gamevariable reference)
    {
        return reference.Value;
    }*/
}
