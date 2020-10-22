using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopMovement : MonoBehaviour, IMovementComponent, IUseGround
{
    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}

    [SerializeField]
    private float sideAirControl = 1;

    [SerializeField]
    private float frontAirControl = 1;

    [SerializeField]
    private int groundLayer;
    public int CollisionLayer {get => groundLayer;}
    public bool touchingGround {get; set;}
    private float input;
    private Vector3 _mov = Vector3.zero;
    private Vector3 _fact = Vector3.one;

    [SerializeField]
    private float forwardHopForce;
    [SerializeField]
    private float upwardHopForce;

    [SerializeField]
    private float landingDragDecelerationTime;

    float z = 0;
    
    private void FixedUpdate() 
    {
        print(touchingGround);
        input = Input.GetAxisRaw("Jump");

        if(input != 0 && touchingGround)
        {
  
            Hop();
            

        }
        else if(touchingGround)
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
        _fact.x = sideAirControl;
        _fact.z = frontAirControl;
        _mov.y = upwardHopForce;
        _mov.z = forwardHopForce;
    }


    

}
