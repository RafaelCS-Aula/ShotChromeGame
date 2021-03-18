// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

/// -----------------
/// Altered by Rafael Castro e Silva.
/// 
/// 22/10/2020
/// ----------------

using UnityEngine;

[CreateAssetMenu(menuName = "Game variables/Int")]
public class IntData : DatabaseVariable<int>
{

    public void ApplyChange(int amount)
    {
        variableValue += amount;
    }

    public void ApplyChange(IntData amount)
    {
        variableValue += amount.Value;
    }

    public static implicit operator int(IntData reference)
    {
        return reference.Value;
    }
}
