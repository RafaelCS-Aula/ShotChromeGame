using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour, IMovementComponent
{
    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}

    private float inputToleranceTime = 0;
    [SerializeField] private AnimationCurve jumpForceFalloff;
    private float jumpMultiplier;
    


    // Start is called before the first frame update
    void Start()
    {
       // Physics.over
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
