using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public class JumpMovement : MovementBase
{
    private GroundChecker _GChecker;
    [SerializeField] 
    private FloatVariable jumpAceleration;

    private bool _input;
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
        
       if(_input && !_GChecker.OnGround())
        {
            
            Jump();

        }
        else if(_GChecker.OnGround())
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
      
        //_fact.x = sideAirControl.Value;
        //_fact.z = frontAirControl.Value;
       _mov.y = jumpAceleration; 
        
    }
}
