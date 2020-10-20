using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGravity : MonoBehaviour, IMovementComponent, IUseGround
{
    [SerializeField]
    private float sideAirControl = 1;

    [SerializeField]
    private float frontAirControl = 1;

    [SerializeField]
    private float gravityAceleration;

    [SerializeField]
    private float terminalVelocity;
    private float _timeFalling = 0;
    private Vector3 _mov; 
    public bool useEngineGravity = false;

    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}

    [SerializeField]
    private int groundLayer;
    public int CollisionLayer {get => groundLayer;}
    public bool touchingGround {get; set;}

    private void Awake() 
    {
        FactorVector = Vector3.one;    
        MovementVector = Vector3.zero;
    }
    private void Update()
    {   
        
        if(!touchingGround)
        {
            
            Fall();
            
        }
        else
        {
            _mov.y = 0;
            _timeFalling = 0;
        }
            
        MovementVector = _mov;
        print(_mov.y);
        

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
