using UnityEngine;

    [CreateAssetMenu(menuName = "Game variables/Vector2")]
    public class Vector2Data : DatabaseVariable<Vector2>
    {

        public static implicit operator Vector2(Vector2Data reference)
        {
            return reference.Value;
        }

    }
