using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FirstPersonCameraSetting : MonoBehaviour
{

    [SerializeField] private KeycodeVariable cursorKey;
    [SerializeField] private FloatVariable fieldOfView;
    
    private bool _cursorLock;

    // Start is called before the first frame update
    void Awake()
    {
        //Dont let it go to build with it like this (recommended)
        Cursor.lockState = CursorLockMode.Locked;
        Camera cam = GetComponent<Camera>();
        if(cam)
            cam.fieldOfView = fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(Input.GetKeyDown(cursorKey))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
                _cursorLock = false;
            if(Cursor.lockState == CursorLockMode.None)
                _cursorLock = true;
             
            
        }

        if(_cursorLock)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        //print(_cursorLock);
    }
}
