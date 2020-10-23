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

    [CreateAssetMenu]
    public class FloatVariable : DatabaseVariable<float>
    {
        public void ApplyChange(float amount)
        {
            variableValue += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            variableValue += amount.Value;
        }
    }
