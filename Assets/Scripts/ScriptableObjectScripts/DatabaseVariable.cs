using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class DatabaseVariable<T> : ScriptableObject
{

    [SerializeField]
    [ResizableTextArea]
    protected string DevDescription; 

    [SerializeField]
    protected T variableValue;
    public virtual T Value {get => variableValue; 
        protected set => value = variableValue;}

    public void SetValue(T value)
    {
        variableValue = value;
        OnValueChanged(value);
    }

    public void SetValue(DatabaseVariable<T> value)
    {
        variableValue = value.Value;
        OnValueChanged(value.Value);
    }

    protected virtual void OnValueChanged(T change)
    {}
}
