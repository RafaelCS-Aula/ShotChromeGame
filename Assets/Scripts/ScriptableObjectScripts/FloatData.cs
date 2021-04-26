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
using System;
using NaughtyAttributes;

    [CreateAssetMenu(menuName = "Game variables/Float")]
    public class FloatData : DatabaseVariable<float>
    {

        [SerializeField]
        private bool WholeNumbers = false;

        [SerializeField][Tooltip("Clamp this value between the bounds")]
        private bool clampValue = false;

        [SerializeField][ShowIf("clampValue")]
        private FloatVariable UpperBound;

        [SerializeField][ShowIf("clampValue")]
        private FloatVariable LowerBound;

        public Vector2 Bounds => new Vector2(LowerBound, UpperBound);

        public override float Value { get => !WholeNumbers ? base.Value : (int)Math.Round(base.Value) ; protected set => base.Value = value; }

        public void ApplyChange(float amount)
        {
            variableValue += amount;
            OnValueChanged(amount);
        }

        public void ApplyChange(int amount)
        {
            variableValue += amount;
            OnValueChanged(amount);
        }


        public void ApplyChange(FloatData amount)
        {
            variableValue += amount.Value;
            OnValueChanged(amount);
        }


        protected override void OnValueChanged(float change)
        {
            if(clampValue)
            {
                if(variableValue > UpperBound)
                    variableValue = UpperBound.Value;
                if(variableValue < LowerBound)
                    variableValue = LowerBound.Value;
            }
            
        }

        public static implicit operator float(FloatData reference)
        {
            return reference.Value;
        }

        public static implicit operator int(FloatData reference)
        {
            return (int)Math.Round(reference.Value);
        }


    }
