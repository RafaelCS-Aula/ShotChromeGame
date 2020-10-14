using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour, IMovementComponent
{
    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}

    [SerializeField] 
    private LateralMovementStats movementStats;
    public LateralMovementStats MovementData {get => movementStats;}

    private float _acceleration;
    private Vector3 _velocity = new Vector3();
    private Vector2 _direction = new Vector3();
    float lastXVel = 0.0f;
        float xDec = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _velocity = Vector3.zero;
    }
    //Temporary
    private void GetInput()
    {
        _direction.x = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        _direction.y = Mathf.Round(Input.GetAxisRaw("Vertical"));
        _direction.Normalize();
    }


    public void AccelerateX()
    {  
        
        
        if(_direction.x != 0)
        {
            
            _velocity.x = Mathf.SmoothDamp(_velocity.x, MovementData.maxStrafeVelocity * _direction.x, ref lastXVel, MovementData.acelerationTime);
            
        }
        else
        {
            //print(lastXVel);
            _velocity.x = Mathf.SmoothDamp(_velocity.x, 0, ref lastXVel, MovementData.decerelationTime);
            
        }
        //lastXVel = 0;
        
    
    }

    public void AccelerateZ()
    {
        
        if(_direction.y != 0)
        {
            float velocityVariable = _direction.y > 1 ? MovementData.maxForwardVelocity : MovementData.maxBackVelocity;

            _velocity.z = Mathf.SmoothDamp(_velocity.z, velocityVariable *  _direction.y, ref xDec, MovementData.acelerationTime);

        }
        else
        {
            _velocity.z = Mathf.SmoothDamp(_velocity.z, 0, ref xDec, MovementData.decerelationTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        AccelerateX();
        AccelerateZ();
        MovementData.currentVelocity = _velocity;
        FactorVector = Vector3.one;
        MovementVector = Vector3.zero;
        MovementVector += transform.forward * _velocity.z;
        MovementVector += transform.right * _velocity.x;
    }

    private void OnDrawGizmos() {
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x * transform.right.x, transform.position.y, transform.position.z * transform.forward.z) * 40);
    }

}
