using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(GroundChecker))]
public class HorizontalMovement : MovementBase
{
    private Vector3 _velocity = new Vector3();
    private Vector2 _direction = new Vector3();
    float lastXVel = 0.0f;
    float xDec = 0.0f;

    private GroundChecker _GChecker;
    
    [Header("Horizontal Movement Settings")]
    [SerializeField]
    private FloatVariable maxForwardVelocity;
       
    [SerializeField]
    private FloatVariable maxBackVelocity;


    [SerializeField]
    private FloatVariable maxStrafeVelocity;

    
    [SerializeField]
    private FloatVariable acelerationTime;

    
   [SerializeField]
    private FloatVariable decelerationTime;


    [Header("Crowd Movement Settings")]

    [SerializeField] private bool canCrowdRun;

    [ShowIf("canCrowdRun")]
    [SerializeField] private LayerMask crowdColliderLayer;
    private Collider _crowdCollider = null;
    private CrowdAgent _inContactComponent = null;

    [ShowIf("canCrowdRun")]
    [SerializeField] private FloatVariable timeBeforeSlowDown;

    [ShowIf("canCrowdRun")]
    [SerializeField] private FloatVariable crowdVelocityModifier;

    [ShowIf("canCrowdRun")]
    [SerializeField] private CurveVariable slowDownCurve;

    //[ShowIf("canCrowdRun")]
    //[SerializeField] private FloatVariable crowdDecelerationTime;

    private float _currentVelMod = 1;
    private bool _onCrowd;
    private RaycastHit _rayhit;
    private float _slowDownTimer;



    
    // Start is called before the first frame update
    private void Start()
    {
        _velocity = Vector3.zero;
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
            InputHolder.InpDirection += GetDirection;
        else if(!UseInput)
            InputHolder.InpDirection -= GetDirection;
    }

    public void GetDirection(Vector2 directionVector)
    {
        _direction.x = directionVector.x;
        _direction.y = directionVector.y;
        _direction.Normalize();
    }


    private void AccelerateX()
    {  
        
        
        if(_direction.x != 0)
        {
            
            _velocity.x = Mathf.SmoothDamp(_velocity.x, maxStrafeVelocity.Value * _direction.x, ref lastXVel, acelerationTime.Value);
            
        }
        else
        {
            //print(lastXVel);
            _velocity.x = Mathf.SmoothDamp(_velocity.x, 0, ref lastXVel, decelerationTime.Value);
            
        }
        //lastXVel = 0;
        
    
    }

    private void AccelerateZ()
    {
        
        if(_direction.y != 0)
        {
            float velocityVariable = _direction.y > 0 ? maxForwardVelocity.Value : maxBackVelocity;

            _velocity.z = Mathf.SmoothDamp(_velocity.z, velocityVariable *  _direction.y, ref xDec, acelerationTime.Value);

        }
        else
        {
            _velocity.z = Mathf.SmoothDamp(_velocity.z, 0, ref xDec, decelerationTime.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canCrowdRun)
        {
            _onCrowd = _GChecker.CheckRay(out _rayhit, crowdColliderLayer);
            if(_onCrowd)
            {
                CrowdContact(_rayhit.collider);

                if(_inContactComponent)
                {
                    // multiply by constant to make it more user friendly to    
                    // evaluate
                    _inContactComponent.EvaluateVelocity(
                                        _velocity.magnitude * 10); 
                }
                
                if(_slowDownTimer >= timeBeforeSlowDown.Value)
                {
                    float currSlow = _slowDownTimer - timeBeforeSlowDown;
                    slowDownCurve.Value.postWrapMode = WrapMode.Clamp;
                    _currentVelMod = 
                        crowdVelocityModifier * 
                        slowDownCurve.Value.Evaluate(currSlow);
                }
                
            }
            else
            {
                _crowdCollider = null;
                _slowDownTimer = 0;
            }



        }
            
   
        

        AccelerateX();
        AccelerateZ();



        FactorVector = Vector3.one;
        MovementVector = _velocity * _currentVelMod;
    }


    private void CrowdContact(Collider contact)
    {
        print(_crowdCollider != _rayhit.collider);
           //print("contact");
            if(_crowdCollider != _rayhit.collider)
            {
                //print("new colldier");
                if(Input.GetAxisRaw  ("Vertical") > 0 && _crowdCollider == null)
                {
                    //print("new colldier, reset timer");
                    _slowDownTimer = 0;
                }
                // Might be performance intensive
                _inContactComponent = 
                    _rayhit.collider.GetComponent<CrowdAgent>();

                _crowdCollider = _rayhit.collider;   
            }
            _slowDownTimer += Time.deltaTime;
            //print("increasing slowDownTimer");
    }
    private void OnDrawGizmos() {
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x * transform.right.x, transform.position.y, transform.position.z * transform.forward.z) * 40);
    }

}
