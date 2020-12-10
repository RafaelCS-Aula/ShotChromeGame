using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(CharacterGravity))]
public class VerticalMovement : MovementBase
{
    [Header("Vertical Settings")]
    
    [SerializeField] 
    private FloatVariable jumpForce;

    [SerializeField] 
    private FloatVariable leapForwardForce;
    [SerializeField] 
    private FloatVariable leapUpwardForce;

    private bool _input;
    private GroundChecker _GChecker;
    // Start is called before the first frame update
    void Start()
    {
       FactorVector = Vector3.one;
       _GChecker = GetComponent<GroundChecker>();
    }

    private void OnEnable()
    {
      
      RegisterForInput();
            
    }

    protected override void RegisterForInput()
    {   
        base.RegisterForInput();
        if(UseInput)
            InputHolder.InpJump += InputDown;
        else if(!UseInput)
            InputHolder.InpJump -= InputDown;
    }
   
    private void InputDown(bool key) => _input = key;
    // Update is called once per frame
    void FixedUpdate()
    {
        
       if(_input && _GChecker.OnGround())
        {
            
            Jump(jumpForce);

        }
        else if(_GChecker.OnGround())
        {
            
            _fact.x = 1;
            _fact.z = 1;
            _mov.y = 0;
            _mov.z = 0;
        }

        
        
            MovementVector = _mov;
            FactorVector = _fact;
        
    }

    public void Jump(float jumpPower)
    {
        // TODO: Make the jump higher if jump key is held down
        /*if(!touchingGround && input != 0)
        {

        }*/
      
        //_fact.x = sideAirControl.Value;
        //_fact.z = frontAirControl.Value;

        print("jump!");
       _mov.y = jumpPower; 
        
    }

    public void Leap(float forwardPower, float upPower)
    {
        _mov.y = upPower;
        _mov.z = forwardPower;
    }

    //TODO: Make a leap where you can just give it a point
    //and it'll calc the forward force. 
    public void LeapToPoint(Vector3 goalPoint)
    {
        float distToPoint = (goalPoint - transform.position).magnitude;
        Vector3 dirToPoint = (goalPoint - transform.position).normalized;
        // float forwardPower = 
    }
}
