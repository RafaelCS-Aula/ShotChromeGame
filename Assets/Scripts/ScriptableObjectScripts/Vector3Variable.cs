using UnityEngine;

    [CreateAssetMenu]
    public class Vector3Variable : DatabaseVariable<Vector3>
    {

        public static implicit operator Vector3(Vector3Variable reference)
        {
            return reference.Value;
        }

    }
