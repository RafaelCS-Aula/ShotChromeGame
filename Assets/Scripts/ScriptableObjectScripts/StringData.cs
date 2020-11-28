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

    [CreateAssetMenu(menuName = "Game variables")]
    public class StringData : DatabaseVariable<string>
    {


        public static implicit operator string(StringData reference)
        {
            return reference.Value;
        }
    }