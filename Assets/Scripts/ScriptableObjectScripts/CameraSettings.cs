using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ConfigurationData/CameraControls", fileName = "CameraControlData")]
[RequireComponent(typeof(Camera))]
public class CameraSettings : ScriptableObject
{
    public Vector2 mouseSensitivity;
    public Vector2 VerticalLimits;
    public Vector2 HorizontalLimits;
    public float fieldOfView;


}
