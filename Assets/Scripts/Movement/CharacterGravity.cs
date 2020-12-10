using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public class CharacterGravity : MonoBehaviour, IMovementComponent
{

    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}
    private Vector3 _mov = Vector3.zero;
    private Vector3 _fact = Vector3.one;

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
           // print("airTime");
            StartCoroutine(ApplyGravity());
            
        }
        else
        {
            //print("groundTime!");
            StopCoroutine(ApplyGravity());
            
            _mov.y = 0;
            _timeFalling = 0;
   
        }

        Vector3 fac = FactorVector;
        FactorVector = fac;
        MovementVector = _mov;  
        
        
    }

    public IEnumerator ApplyGravity()
    {
       /* if(_GChecker.OnGround())
            yield break;*/
        
        _timeFalling += Time.fixedDeltaTime;
        float fallVelocity = _fallForce * _timeFalling;
        _mov.y = -fallVelocity;
        yield return null;
    }




}
