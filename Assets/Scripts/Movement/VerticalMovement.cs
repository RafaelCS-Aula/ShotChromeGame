using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(CharacterGravity))]
public class VerticalMovement : MovementBase
{
    [Foldout("Events")]
    [SerializeField] private UnityEvent OnHopEvent;
    [Foldout("Events")]
    [SerializeField] private UnityEvent OnJumpEvent;
    [Foldout("Events")]
    [SerializeField] private UnityEvent OnLandEvent;

    private GroundChecker _GChecker;
    private CharacterGravity _cGrav;
    private float _gravPull;
    [Header("Vertical Settings")]
    
    [SerializeField] 
    private FloatVariable jumpForce;

    


    [Header("Crowd Hopping Settings")]
    [SerializeField] private bool canCrowdHop;

    [ShowIf("canCrowdHop")]
    [SerializeField] private bool steppingOffHop;
    
    [ShowIf("canCrowdHop")]
    [SerializeField] 
    private FloatVariable hopUpwardForce;
    
    [ShowIf("canCrowdHop")]
    [SerializeField] 
    private FloatVariable hopForwardForce;

    [ShowIf("canCrowdHop")]
    [SerializeField] 
    private FloatVariable hopLeapUpForce;

    [ShowIf("canCrowdHop")]
    [SerializeField] 
    private FloatVariable hopLeapForwardForce;

    private RaycastHit _rayhit;

    [ShowIf("canCrowdHop")]
    [SerializeField] private LayerMask hoppingLayer;

    private Collider _currentCrowdCollider;
    private Collider _newCrowdCollider;
    
    // Start is called before the first frame update
    void Start()
    {
       FactorVector = Vector3.one;
       _GChecker = GetComponent<GroundChecker>();
       _cGrav = GetComponent<CharacterGravity>();
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

        if(canCrowdHop)
            CheckForCrowd();

        _gravPull = _cGrav.gravitationalPull;
       if(_input && _GChecker.OnGround())
        {
            
            Jump(jumpForce);

        }
        else if(_GChecker.OnGround())
        {
            // Was moving vertically before hitting ground
            if(_mov.y != 0)
                OnLandEvent.Invoke();
            
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
        OnJumpEvent.Invoke();
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
    
    /*public void LeapToPoint(Vector3 goalPoint, float secondsToReach)
    {
        float distToPoint = (goalPoint - transform.position).magnitude;
        Vector3 dirToPoint = (goalPoint - transform.position).normalized;

        Quaternion.LookRotation(dirToPoint, transform.up);

        float forwardPower = distToPoint / secondsToReach;
        float upPower = _gravPull;

        _mov.y = upPower;
        _mov.z = forwardPower;

    }

    [Button]
    private void TestLeapToPoint()
    {
       // _gravPull = GetComponent<CharacterGravity>().gravitationalPull;
        LeapToPoint(Vector3.zero, 1);
    }*/

    private void CheckForCrowd()
    {
        if(_GChecker.CheckRay(out _rayhit, hoppingLayer))
                _newCrowdCollider = _rayhit.collider;
            else
                _newCrowdCollider = null;
        
        // new head
        if(_currentCrowdCollider != _newCrowdCollider)
        {
            print("new head");
            OnHopEvent.Invoke();
            if(_input)
                Leap(hopLeapForwardForce, hopLeapUpForce);
            else
                Leap(hopForwardForce, hopUpwardForce);

        }
        _currentCrowdCollider = _newCrowdCollider;

    }
}
