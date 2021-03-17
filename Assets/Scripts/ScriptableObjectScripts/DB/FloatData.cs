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

    [CreateAssetMenu(menuName = "Game variables/Float")]
    public class FloatData : DatabaseVariable<float>
    {
        
        public void ApplyChange(float amount)
        {
            variableValue += amount;
        }

        public void ApplyChange(FloatData amount)
        {
            variableValue += amount.Value;
        }

        public static implicit operator float(FloatData reference)
        {
            return reference.Value;
        }

    }
