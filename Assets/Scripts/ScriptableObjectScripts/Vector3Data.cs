﻿using UnityEngine;

    [CreateAssetMenu(menuName = "Game variables")]
public class Vector3Data : DatabaseVariable<Vector3>
    {

        public static implicit operator Vector3(Vector3Data reference)
        {
            return reference.Value;
        }

    }
