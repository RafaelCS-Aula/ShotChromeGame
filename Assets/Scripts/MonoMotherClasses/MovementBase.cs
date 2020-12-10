using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class MovementBase : MonoBehaviour, IMovementComponent, ISeekInput
{

    protected InputManager _inpManager;
    public InputManager InputHolder
    {
        get => _inpManager; 
        set => value = _inpManager;
    }

    [OnValueChanged("OnEnable")]
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


    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}
    protected Vector3 _mov = Vector3.zero;
    protected Vector3 _fact = Vector3.one;




}
