using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class InputReceiverBase : MonoBehaviour, ISeekInput
{
    
    protected InputManager _inpManager;
    public InputManager InputHolder
    {
        get => _inpManager; 
        set => value = _inpManager;
    }
    
    [Header("Input Settings")]
    [OnValueChanged("RegisterForInput")]
    [Label("Use Input")] // Maintain naming conventions but keep inspector neat
    [SerializeField] protected bool _useInput;
    public bool UseInput {get => _useInput; set => value = _useInput;}

    
    protected virtual void RegisterForInput()
    {
        if(InputHolder == null)
            _inpManager = GetComponent<InputManager>();

        if(InputHolder == null) // There is no InputHolder
            return;
        
    }
}
