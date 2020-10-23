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

using System;

    [Serializable]
    public class DatabaseReference<T>
    {
        // Altered to initialise to false
        public bool UseConstant = false;
        public T ConstantValue;
        public DatabaseVariable<T> Variable;

        public DatabaseReference()
        {   }

        public DatabaseReference(T value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public T Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        /*public static implicit operator S(T reference)
        {
            return reference.Value;
        }*/
    }
