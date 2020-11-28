using UnityEngine;

    [CreateAssetMenu(menuName = "Game variables")]
    public class Vector2Data : DatabaseVariable<Vector2>
    {

        public static implicit operator Vector2(Vector2Data reference)
        {
            return reference.Value;
        }

    }
