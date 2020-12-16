using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class MouseRotation : MonoBehaviour
{

    private float _mouseDeltaX = 0;
    private float _mouseDeltaY = 0;
    private float _horizontalDelta;
    private float _verticalDelta;

    [SerializeField] private BoolVariable needMouseLock;
    private Quaternion _startRotation;

    [ValidateInput("HasNeededHVars", "Input horizontal sensitivity and angle limit")]
    [SerializeField] private bool turnHorizontal;

    [ValidateInput("HasNeededVVars", "Input vertical sensitivity and angle limit")]
    [SerializeField] private bool turnVertical;

    [ShowIf("turnHorizontal")]
    [SerializeField] private FloatVariable horizontalSensitivity;

    [ShowIf("turnVertical")]
    [SerializeField] private FloatVariable verticalSensitivity;

    [ShowIf("turnHorizontal")]
    [SerializeField] private FloatVariable horizontalMaxAngle;

    [ShowIf("turnVertical")]
    [SerializeField] private FloatVariable verticalMaxAngle;

#region Naughty Attributes helper methods
    private bool HasNeededHVars()
    {
        if(turnHorizontal)
        {
            return (horizontalMaxAngle != null && horizontalSensitivity != null);
        }
        else
           return true;
    }
    private bool HasNeededVVars()
    {
        if(turnVertical)
            return (verticalMaxAngle != null && verticalSensitivity != null);
        else return true;
    }
#endregion


    private void Start() 
    {
        _startRotation = transform.rotation;

        
    }
    void Update()
    {
        if(needMouseLock && Cursor.lockState != CursorLockMode.Locked)
            return;

       // Apply trunign restrictions
       if(turnHorizontal)
       {
           _mouseDeltaX = 
            Input.GetAxisRaw("Mouse X") * horizontalSensitivity * 
                Time.deltaTime;
            _horizontalDelta += _mouseDeltaX;
            ApplyHorizontalRestrictions();
            transform.Rotate(Vector3.up * _mouseDeltaX);
       }

        if(turnVertical)
        {
            _mouseDeltaY = 
            Input.GetAxisRaw("Mouse Y") * verticalSensitivity *
                Time.deltaTime;

            _verticalDelta += _mouseDeltaY;
            
            ApplyVerticalRestrictions();
            transform.Rotate(Vector3.left * _mouseDeltaY);
        }

    }


    private void ApplyHorizontalRestrictions()
    {
        if(horizontalMaxAngle != 0)
        {
            if(_horizontalDelta > horizontalMaxAngle )
            {
                _horizontalDelta = horizontalMaxAngle;
                _mouseDeltaX = 0;
                ClampHorizontalRotation(360 - horizontalMaxAngle);
            }
            else if(_horizontalDelta < -horizontalMaxAngle)
            {   
                _horizontalDelta = -horizontalMaxAngle;
                _mouseDeltaX = 0;
                ClampHorizontalRotation(horizontalMaxAngle);
            }

        }

    }

    private void ApplyVerticalRestrictions()
    {
        if(verticalMaxAngle != 0)
        {
            if(_verticalDelta > verticalMaxAngle )
            {
                _verticalDelta = verticalMaxAngle;
                _mouseDeltaY = 0;
                ClampVerticalRotation(360 - verticalMaxAngle);
            }

            else if(_verticalDelta < -verticalMaxAngle)
            {   
                _verticalDelta = -verticalMaxAngle;
                _mouseDeltaY = 0;
                ClampVerticalRotation(verticalMaxAngle);
            }

        } 

    }
    private void ClampHorizontalRotation(float degrees)
    {
        Vector3 rot = transform.eulerAngles;
        rot.y = degrees;
        transform.eulerAngles = rot;
    }
    private void ClampVerticalRotation(float degrees)
    {
        Vector3 rot = transform.eulerAngles;
        rot.x = degrees;
        transform.eulerAngles = rot;
    }
    
}
