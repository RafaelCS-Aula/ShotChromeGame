using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HorizontalMovement : MonoBehaviour, IMovementComponent
{
    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}

    /*[SerializeField] 
    private LateralMovementStats movementStats;
    public LateralMovementStats MovementData {get => movementStats;}*/

    [Expandable]
    [SerializeField]
    private FloatVariable maxStrafeVelocity;

    [SerializeField]
    private FloatVariable acelerationTime;

   [SerializeField]
    private FloatVariable decelerationTime;

    [SerializeField]
    private FloatVariable maxForwardVelocity;

    [SerializeField]
    private FloatVariable maxBackVelocity;



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
            
            _velocity.x = Mathf.SmoothDamp(_velocity.x, maxStrafeVelocity.Value * _direction.x, ref lastXVel, acelerationTime.Value);
            
        }
        else
        {
            //print(lastXVel);
            _velocity.x = Mathf.SmoothDamp(_velocity.x, 0, ref lastXVel, decelerationTime.Value);
            
        }
        //lastXVel = 0;
        
    
    }

    public void AccelerateZ()
    {
        
        if(_direction.y != 0)
        {
            float velocityVariable = _direction.y > 1 ? maxForwardVelocity.Value : maxBackVelocity.Value;

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
        GetInput();
        AccelerateX();
        AccelerateZ();
        FactorVector = Vector3.one;
        MovementVector = _velocity;
    }

    private void OnDrawGizmos() {
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x * transform.right.x, transform.position.y, transform.position.z * transform.forward.z) * 40);
    }

}
