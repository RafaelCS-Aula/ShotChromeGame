using UnityEngine;

    [CreateAssetMenu]
    public class Vector2Variable : DatabaseVariable<Vector2>
    {

        public static implicit operator Vector2(Vector2Variable reference)
        {
            return reference.Value;
        }

    }
