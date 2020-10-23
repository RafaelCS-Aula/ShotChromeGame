using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

public class HopMovement : VerticalMovementBase
{
    
    [SerializeField]
    private float forwardHopForce;
    [SerializeField]
    private float upwardHopForce;

    [SerializeField]
    private float landingDragDecelerationTime;

 
    
    private void FixedUpdate() 
    {
        //print(touchingSpecialGround);
        input = Input.GetAxisRaw("Jump");

        /*if(input != 0 && touchingSpecialGround)
        {
  
            Hop();
            

        }
        else if(touchingNonSpecialGround)
        {   
            _fact.x = 1;
            _fact.z = 1;
            _mov.y = 0;
            _mov.z = 
                Mathf.SmoothDamp(_mov.z, 0, ref z, landingDragDecelerationTime);
        }*/
        
        
  
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


    

}
