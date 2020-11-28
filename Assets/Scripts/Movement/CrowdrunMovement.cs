using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrowdrunMovement : VerticalMovementBase
{
    [Header("Crowd Running")]
   
    [SerializeField] Vector3Variable currentVelocity;

    private float _velocity {get => currentVelocity.Value.sqrMagnitude;}


    [SerializeField] private FloatVariable slowDownAmount;

 


    [SerializeField] FloatVariable timeBeforeSlowDown;

    private float _slowDownTimer = 0;

    [SerializeField] private LayerMask crowdColliderLayer;


    [SerializeField] private FloatVariable crowdDetectorRayLength;

    private Collider _crowdCollider = null;
    private RaycastHit _rayhit;
    private CrowdAgent _inContactComponent = null;

    //private Ray crowdDetectorRay;
    float slowDownDuration = 0.5f;

    float slow;

    // Update is called once per frame
    void FixedUpdate()
    {
        ContactCrowd();

        if(_slowDownTimer >= timeBeforeSlowDown?.Value)
        {


            // slow down the runner
            if(currentVelocity.Value.z > 0)
                _mov.z = -slowDownAmount * _slowDownTimer;
            else
                _mov.z = 0;
            //_mov.z = Mathf.Clamp(_mov.z, 0, -_velocity); 
            //_fact *= 0.9f;
            /*_mov.z = 
                Mathf.SmoothDamp(_mov.z, -currentVelocity.Value.z, ref slow, slowDownDuration);*/



        }
        
            if(_inContactComponent)
            _inContactComponent.EvaluateVelocity(_velocity);

        MovementVector = _mov;
        FactorVector = _fact;

       // print(_velocity);
    }

    private void ContactCrowd()
    {
        
        bool contact;
        contact = 
            Physics.Raycast(
                transform.localPosition + feetPosition, Vector3.down,
                hitInfo: out _rayhit, crowdDetectorRayLength.Value, crowdColliderLayer);
        
        if(contact)
        {
            print(_crowdCollider != _rayhit.collider);
           //print("contact");
            if(_crowdCollider != _rayhit.collider)
            {
                print("new colldier");
                if(Input.GetAxisRaw  ("Vertical") > 0 && _crowdCollider == null)
                {
                    print("new colldier, reset timer");
                    _slowDownTimer = 0;
                }

                // Might be performance intensive
                _inContactComponent = 
                    _rayhit.collider.GetComponent<CrowdAgent>();

                _crowdCollider = _rayhit.collider;
                    
                
            }
            _slowDownTimer += Time.deltaTime;
            print("increasing slowDownTimer");
                
            
            
        }
        else 
        {
            print("no contact");
            _crowdCollider = null;
            _slowDownTimer = 0;
            
        }
    }


    private new void OnDrawGizmos() 
    {
        if(showGizmos)
            Gizmos.DrawLine(transform.localPosition + feetPosition, transform.localPosition + feetPosition.Value * crowdDetectorRayLength.Value);    
    }
}
