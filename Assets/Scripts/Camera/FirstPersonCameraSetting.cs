using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FirstPersonCameraSetting : MonoBehaviour
{

    [SerializeField] private KeycodeVariable cursorKey;
    [SerializeField] private FloatVariable fieldOfView;
    
    private bool _cursorLock;
    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        //Dont let it go to build with it like this (recommended)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
       
        //if (cam) cam.fieldOfView = fieldOfView;
        //print(cam.fieldOfView);
    }
    

    // Update is called once per frame
    void Update()
    {
       if (cam) cam.fieldOfView = fieldOfView;
    }
}
