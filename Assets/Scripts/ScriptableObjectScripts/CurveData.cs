using UnityEngine;

    [CreateAssetMenu(menuName = "Game variables/Curve")]
    public class CurveData : DatabaseVariable<AnimationCurve>
    {

        public static implicit operator AnimationCurve(CurveData reference)
        {
            return reference.Value;
        }

    }
