using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HopMovement : VerticalMovementBase
{
    [Header("Hop Settings")]
    [HorizontalLine(color: EColor.Yellow)]
    [SerializeField]
    [Expandable] private FloatVariable forwardHopForce;

    [SerializeField]
    [Expandable] private FloatVariable upwardHopForce;

    [SerializeField]
    [Expandable] private FloatVariable landingDragDecelerationTime;

    [Header("Unique Collision Settings")]
    [SerializeField] 
    private LayerMask hoppingMask;

    [SerializeField]
    [Expandable] private FloatVariable hopSpotDetectorRadius;

    [SerializeField]
    [Expandable] private Vector3Variable hopSpotDetectorPosition;



    private float z;
    
    private void FixedUpdate() 
    {
        //print(touchingSpecialGround);
        input = Input.GetAxisRaw("Jump");

        if(input != 0 && TouchingHopSpot())
        {
  
            Hop();
            

        }
        else if(TouchingGround())
        {   
            _fact.x = 1;
            _fact.z = 1;
            _mov.y = 0;
            _mov.z = 
                Mathf.SmoothDamp(_mov.z, 0, ref z, landingDragDecelerationTime);
        }
        
        
  
        MovementVector = _mov;
        FactorVector = _fact;

    }

    public void Hop()
    {
        _fact.x = sideAirControl.Value;
        _fact.z = frontAirControl.Value;
        _mov.y = upwardHopForce;
        _mov.z = forwardHopForce;
    }

    private bool TouchingHopSpot()
    {
        return Physics.OverlapSphere(hopSpotDetectorPosition, hopSpotDetectorRadius, hoppingMask).Length > 0;
    }


    

}
