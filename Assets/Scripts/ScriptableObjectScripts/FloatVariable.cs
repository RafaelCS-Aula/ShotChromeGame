// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

/// -----------------
/// Altered by Rafael Castro e Silva.
/// 
/// 2020
/// ----------------

using UnityEngine;

    [CreateAssetMenu]
    public class FloatVariable : DatabaseVariable
    {
        [SerializeField]
        private float floatValue;

        public float Value {get => floatValue; 
        private set => value = floatValue;}

        public void SetValue(float value)
        {
            floatValue = value;
        }

        public void SetValue(FloatVariable value)
        {
            floatValue = value.Value;
        }

        public void ApplyChange(float amount)
        {
            floatValue += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            floatValue += amount.Value;
        }
    }
