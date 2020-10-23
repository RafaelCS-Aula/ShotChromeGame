using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DatabaseVariable<T> : ScriptableObject
{

    [SerializeField]
    [Multiline]
    protected string DevDescription; 

    [SerializeField]
    protected T variableValue;
    public T Value {get => variableValue; 
        protected set => value = variableValue;}

    public void SetValue(T value)
    {
        variableValue = value;
    }

    public void SetValue(DatabaseVariable<T> value)
    {
        variableValue = value.Value;
    }

}
