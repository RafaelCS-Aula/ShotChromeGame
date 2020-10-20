using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour, IMovementComponent, IUseGround
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

    /*[Tooltip("How long after the jump input will the character still jump")]
    [SerializeField] private float inputToleranceTime;
    private float _inputToleranceCounter;*/
   
    [SerializeField] private float jumpAceleration;
    private float input;
    private Vector3 _mov = Vector3.zero;
    private Vector3 _fact = Vector3.one;


    // Start is called before the first frame update
    void Start()
    {
       FactorVector = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxisRaw("Jump");

        if(input != 0)
        {
            if(touchingGround)
            {
                Jump();
            }

        }
        else if(input == 0 && touchingGround)
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
        
        _fact.x = sideAirControl;
        _fact.z = frontAirControl;
        _mov.y = jumpAceleration;
    }
}
