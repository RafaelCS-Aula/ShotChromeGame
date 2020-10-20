using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGravity : MonoBehaviour, IMovementComponent
{
    public float timeToTerminal;
    public float gravityValue;
    public float terminalVelocity;

    private float _timeFalling;
    private Vector3 _mov; 
    public bool useEngineGravity = false;
    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}
    float vel;
    private void Awake() 
    {
        FactorVector = Vector3.one;    
        MovementVector = Vector3.zero;
    }
    private void Update()
    {   

        Fall();
        MovementVector = _mov;
    }

    public void Fall()
    {
        _timeFalling += Time.deltaTime;
        _mov.y = gravityValue * _timeFalling;
        _mov.y = Mathf.Clamp(_mov.y, gravityValue, terminalVelocity);

    }
}
