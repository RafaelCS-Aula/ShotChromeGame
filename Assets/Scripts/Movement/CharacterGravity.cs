using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(GroundChecker))]
public class CharacterGravity : MonoBehaviour, IMovementComponent
{

    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}
    private Vector3 _mov = Vector3.zero;
    private Vector3 _fact = Vector3.one;

    [Header("Gravity Settings")]

    [MinValue(0.01f)]
    [SerializeField]
    private FloatVariable secondsToTerminalVelocity;

    [SerializeField]
    private FloatVariable terminalVelocity;

    private float _timeFalling = 0;
    
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

        
        float fallVelocity = useEngineGravity ? 
            (Physics.gravity.y * _timeFalling) : 
            (terminalVelocity * (_timeFalling / secondsToTerminalVelocity));


        if(fallVelocity > terminalVelocity)
            fallVelocity = terminalVelocity;

        _mov.y = -fallVelocity;
        yield return null;
    }




}
