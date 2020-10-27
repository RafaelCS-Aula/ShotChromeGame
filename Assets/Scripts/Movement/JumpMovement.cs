﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class JumpMovement : VerticalMovementBase
{
    

    /*[Tooltip("How long after the jump input will the character still jump")]
    [SerializeField] private float inputToleranceTime;
    private float _inputToleranceCounter;*/

    [Header("Jump Settings")]
    [HorizontalLine(color: EColor.Yellow)]

    [Expandable]
    [SerializeField] 
    private FloatVariable jumpAceleration;



    // Start is called before the first frame update
    void Start()
    {
       FactorVector = Vector3.one;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        input = Input.GetAxisRaw("Jump");

        if(input != 0 && TouchingGround())
        {
            
            Jump();

        }
        else if(TouchingGround())
        {
            
            _fact.x = 1;
            _fact.z = 1;
            _mov.y = 0;
        }

        
        
            MovementVector = _mov;
            FactorVector = _fact;        
        
    }

    public void Jump()
    {
        // TODO: Make the jump higher if jump key is held down
        /*if(!touchingGround && input != 0)
        {

        }*/
        
        _fact.x = sideAirControl.Value;
        _fact.z = frontAirControl.Value;
        _mov.y = jumpAceleration;
        
    }
}
