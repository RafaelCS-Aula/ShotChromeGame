﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CrowdrunMovement : VerticalMovementBase
{
    [Header("Crowd Running")]
    [HorizontalLine(color: EColor.Yellow)]
    [SerializeField] Vector3Variable currentVelocity;

    private float _forwardVelocity {get => currentVelocity.Value.z;}

    [Expandable]
    [SerializeField] private FloatVariable slowDownAmount;

 

    [Expandable]
    [SerializeField] FloatVariable timeBeforeSlowDown;

    private float _slowDownTimer = 0;

    [SerializeField] private LayerMask crowdColliderLayer;

    [Expandable]
    [SerializeField] private FloatVariable crowdDetectorRayLength;

    private Collider _crowdCollider = null;
    private RaycastHit _rayhit;
    private CrowdAgent _inContactComponent = null;

    //private Ray crowdDetectorRay;


    // Update is called once per frame
    void FixedUpdate()
    {
        ContactCrowd();

        if(_slowDownTimer >= timeBeforeSlowDown)
        {

            // slow down the runner
            _mov.z = -slowDownAmount * _slowDownTimer;
            //_mov.z = Mathf.Clamp(_mov.z, 0, _forwardVelocity); 
            if(_inContactComponent)
            _inContactComponent.EvaluateVelocity(
                currentVelocity.Value.magnitude);

        }

        
        MovementVector = _mov;
        FactorVector = _fact;

        print(_forwardVelocity);
    }

    private void ContactCrowd()
    {
        
        bool contact;
        contact = 
            Physics.Raycast(
                transform.localPosition + feetPosition, Vector3.down,
                hitInfo: out _rayhit, crowdDetectorRayLength, crowdColliderLayer);
        
        if(contact)
        {
            
           //print("contact");
            if(_crowdCollider != _rayhit.collider)
            {
                if(Input.GetAxisRaw  ("Vertical") > 0 && _inContactComponent == null)
                {
                    _slowDownTimer = 0;
                }

                // Might be performance intensive
                _inContactComponent = 
                    _rayhit.collider.GetComponent<CrowdAgent>();
                _crowdCollider = _rayhit.collider;
                
                
            }
            else 
            {
                _slowDownTimer += Time.deltaTime;
                return;
            }
            
        }
        else 
        {
            _inContactComponent = null;
            _slowDownTimer = 0;
            return;
        }
    }


    private void OnDrawGizmos() 
    {
        if(showGizmos)
            Gizmos.DrawLine(transform.localPosition + feetPosition, transform.localPosition + feetPosition.Value * crowdDetectorRayLength);    
    }
}
