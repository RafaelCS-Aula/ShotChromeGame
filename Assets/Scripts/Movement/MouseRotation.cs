using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{

    private float _mouseDeltaX;
    private float _mouseDeltaY;
    private float _horizontalDelta;
    private float _verticalDelta;
    private Quaternion _startRotation;
    [SerializeField] private bool turnHorizontal;
    [SerializeField] private bool turnVertical;


    [SerializeField] private float horizontalSensitivity;
    [SerializeField] private float verticalSensitivity;

    [SerializeField] private float horizontalMaxAngle = 0.0f;
    [SerializeField] private float verticalMaxAngle = 0.0f;


    private void Start() 
    {
        _startRotation = transform.rotation;
      
    }
    void Update()
    {
        _mouseDeltaX = 
            Input.GetAxisRaw("Mouse X") * horizontalSensitivity * 
                Time.deltaTime;
        _mouseDeltaY = 
            Input.GetAxisRaw("Mouse Y") * verticalSensitivity *
                Time.deltaTime;

        _horizontalDelta += _mouseDeltaX;
        _verticalDelta += _mouseDeltaY;

       
       // Apply trunign restrictions
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
       
        //Debug.ClearDeveloperConsole();
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
       

        if(turnHorizontal)
            transform.Rotate(Vector3.up * _mouseDeltaX);
        if(turnVertical)
            transform.Rotate(Vector3.left * _mouseDeltaY);

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
