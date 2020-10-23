using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CharacterGravity : VerticalMovementBase
{

    [Header("Gravity Settings")]
    [HorizontalLine(color: EColor.Yellow)]
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
         useEngineGravity ? Physics.gravity.y : gravityAceleration;

        _timeFalling += Time.deltaTime;
        fallForce *= _timeFalling;
        

        if(fallForce >= terminalVelocity)
            fallForce = terminalVelocity;
            
        //print(fallForce);
        FactorVector = new Vector3(sideAirControl, 1, frontAirControl);
        _mov.y = -fallForce;
    }




}
