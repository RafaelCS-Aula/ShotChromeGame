using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterGravity : VerticalMovementBase
{

    [Header("Gravity Settings")]

    [SerializeField]
    private FloatVariable gravityAceleration;

    [SerializeField]
    private FloatVariable terminalVelocity;
    private float _timeFalling = 0;
    public bool useEngineGravity = false;

    private void Awake() 
    {
        FactorVector = Vector3.one;    
        MovementVector = Vector3.zero;
    }
    private void FixedUpdate()
    {   
        
        if(!TouchingGround())
        {
            
            Fall();
            
        }
        else
        {
            _mov.y = 0;
            _timeFalling = 0;
            Vector3 fac = FactorVector;
            fac.y = 0;
            FactorVector = fac;
        }
            
        MovementVector = _mov;
        //print(_mov.y);
        

    }

    public void Fall()
    {
        float fallForce = 
         useEngineGravity ? Physics.gravity.y : gravityAceleration.Value;

        _timeFalling += Time.deltaTime;
        fallForce *= _timeFalling;
        

        if(fallForce >= terminalVelocity.Value)
            fallForce = terminalVelocity.Value;
            
        //print(fallForce);
        FactorVector = new Vector3(sideAirControl.Value, 1, frontAirControl.Value);
        _mov.y = -fallForce;
    }




}
