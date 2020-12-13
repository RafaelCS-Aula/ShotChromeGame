using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[RequireComponent(typeof(MeshCollider))]
public class CrowdAgent : MonoBehaviour
{

    private Collider _myCol;

    [SerializeField] FloatVariable velocityThreshold;
    [SerializeField] FloatVariable runnerMaxVelocity;

    [SerializeField] FloatVariable timeUntilColiderReset;

    
    
    //private float _factoredThreshold;

    [SerializeField] private bool showGizmos;

    [ShowIf("showGizmos")]
    [SerializeField] private Color gizmoDefaultColor;

    [ShowIf("showGizmos")]
    [SerializeField] private Gradient gizmoSteppedOnColor;
    [ShowIf("showGizmos")]
    [SerializeField] private Color gizmoDisabledColor;

    [ShowIf("showGizmos")]
    [Range(0,1)]
    [SerializeField] private float gizmoTransparency;

    private float _dGivenVelocity = 0;
    private  bool _dInContact = false;
    private  bool _allowRunner = true;

    private void Awake()
    {
        _myCol = GetComponent<MeshCollider>();    

        
        
    }
    public void EvaluateVelocity(float velocity)
    {
        //_factoredThreshold =
        //     runnerMaxVelocity * velocityThreshold;
        //print(velocity);

        //print("Received Velocity: " + velocity);
        _dGivenVelocity = velocity;
        _dInContact = true;

        if(velocity < velocityThreshold)
            _allowRunner = false;
        else
            _allowRunner = true;

        StartCoroutine(ResetCollider());
    }
    private void Update() 
    {
        _dInContact = false;
        _myCol.enabled = _allowRunner;
        //print(_allowRunner);
    }
    private IEnumerator ResetCollider()
    {
        yield return new WaitForSeconds(timeUntilColiderReset);
        _allowRunner = true;
        //print("collider ON");
        yield return null;
    } 

    private void OnDrawGizmos() 
    {
        gizmoDefaultColor.a = gizmoTransparency;
       // gizmoSteppedOnColor. = gizmoTransparency;
        gizmoDisabledColor.a = gizmoTransparency;
        
        if(!showGizmos)
            return;
        /*if(_dInContact)
            Gizmos.color = 
                gizmoSteppedOnColor.Evaluate(_dGivenVelocity/velocityThreshold);
        else
        {*/
            if(_allowRunner)
                Gizmos.color = gizmoDefaultColor;
            else if(!_allowRunner)
                Gizmos.color = gizmoDisabledColor;
        //}
            
        Gizmos.DrawCube(transform.position + transform.up * 1, new Vector3(0.5f,0.5f,0.5f));

    }
}
