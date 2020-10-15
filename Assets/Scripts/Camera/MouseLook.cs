using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseLook : MonoBehaviour
{

    [SerializeField]private CameraSettings _settings;

    private Camera _myCam;

    private float _mouseDeltaX;
    private float _mouseDeltaY;


    // Start is called before the first frame update
    void Awake()
    {
        _myCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        _mouseDeltaX = 
            Input.GetAxisRaw("Mouse X") * _settings.mouseSensitivity.x;
        _mouseDeltaY = 
            Input.GetAxisRaw("Mouse Y") * _settings.mouseSensitivity.y;

        Quaternion yRot = Quaternion.AngleAxis(-_mouseDeltaY, transform.right);
        Quaternion xRot = Quaternion.AngleAxis(_mouseDeltaX, transform.up);
        //Vector3 axis = transform.right;
        //nRot.ToAngleAxis(out _mouseDeltaY, out axis);
        
        transform.rotation = transform.rotation * yRot;
    }
}
