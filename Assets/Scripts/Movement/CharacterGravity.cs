using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public class CharacterGravity : MovementBase
{

    [Header("Gravity Settings")]

    [SerializeField]
    private FloatVariable gravityAceleration;

    [SerializeField]
    private FloatVariable terminalVelocity;

    //[SerializeField] private CurveVariable 
    private float _timeFalling = 0;
    private float _fallForce =>  useEngineGravity ? Physics.gravity.y : gravityAceleration.Value;
    [SerializeField] private bool useEngineGravity = false;

    private GroundChecker _GChecker;

    private void Awake() 
    {
        _GChecker = GetComponent<GroundChecker>();
        FactorVector = Vector3.one;    
        MovementVector = Vector3.zero;
    }
    private void FixedUpdate()
    {   
        
        if(!_GChecker.OnGround())
        {
            print("airTime");
            StartCoroutine(ApplyGravity());
            
        }
        else
        {
            print("groundTime!");
            StopCoroutine(ApplyGravity());
            
            _mov.y = 0;
            _timeFalling = 0;
   
        }

        Vector3 fac = FactorVector;
        FactorVector = fac;
        MovementVector = _mov;  
        
        
    }

    /*public void Fall()
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
    }*/

    public IEnumerator ApplyGravity()
    {
       /* if(_GChecker.OnGround())
            yield break;*/
        
        _timeFalling += Time.fixedDeltaTime;
        float fallVelocity = _fallForce * _timeFalling;
       // print("falling x" + _timeFalling);
        _mov.y = -fallVelocity;
        yield return null;
    }




}
