using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;


public class CrowdAgent : MonoBehaviour
{

    [Foldout("Events")]
    [SerializeField] private UnityEvent OnExpireRunnerEvent;
    private Collider _myCol;
    private AreaofEffect _friendSearchAoE;

    [SerializeField] private bool useProximityEffects;

    [SerializeField] FloatVariable velocityThreshold;
    [SerializeField] FloatVariable timeUntilColiderReset;

    //[SerializeField] FloatVariable timeUntilColiderReset;

    [ShowIf("useProximityEffects")]
    [SerializeField] private IntVariable maxFriendsForEffect;
    [ShowIf("useProximityEffects")]
    [SerializeField] private CurveVariable tresholdCrowdDensityCurve;
    
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
    private float _finalVelocityThreshold;
    private int _friendsNear;

    private void Awake()
    {
        _myCol = GetComponent<Collider>();   
        if(useProximityEffects)
        {
            _friendSearchAoE = GetComponent<AreaofEffect>();
            if(_friendSearchAoE == null)
                Debug.LogError("No Area of Effect Component for crowd agent " + gameObject.name);
        }
        if(_myCol == null)
            Debug.LogError("No collider for crowd agent " + gameObject.name);
        
        // Cube Test
        //_myCol = GetComponent<BoxCollider>();
        
        
    }

    public void EvaluateVelocity(float velocity)
    {
        //_factoredThreshold =
        //     runnerMaxVelocity * velocityThreshold;
        //print(velocity);
        if(useProximityEffects)
        {
            _finalVelocityThreshold = velocityThreshold + tresholdCrowdDensityCurve.Value.Evaluate(_friendsNear / maxFriendsForEffect);
        }
        else
            _finalVelocityThreshold = velocityThreshold;
        //print("Received Velocity: " + velocity);
        _dGivenVelocity = velocity;
        _dInContact = true;

        if(velocity < _finalVelocityThreshold)
        {
            if(useProximityEffects)
            {
                foreach(Collider c in _friendSearchAoE.HitsAndAffect.Keys)
                {
                
                    CrowdAgent a = c.GetComponent<CrowdAgent>();
                    if(a)
                    {
                        a._allowRunner = false;
                        a.OnExpireRunnerEvent.Invoke();
                        a.StartCoroutine(ResetCollider());
                    }
                    
                }

            }
            
            _allowRunner = false;
        }
            
        else
            _allowRunner = true;

        StartCoroutine(ResetCollider());
    }

    // This method can cause lag spikes
    public void GetFriends()
    {
        if(!useProximityEffects)
            return;
        _friendSearchAoE.ApplyAoE(transform.position);
        _friendsNear = _friendSearchAoE.HitsAndAffect.Count;
        print("called friend search");

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
        if(_dInContact)
            Gizmos.color = 
                gizmoSteppedOnColor.Evaluate(velocityThreshold/_finalVelocityThreshold);
        else
        {
            if(_allowRunner)
                Gizmos.color = gizmoDefaultColor;
            else if(!_allowRunner)
                Gizmos.color = gizmoDisabledColor;
        }
            
        Gizmos.DrawCube(transform.position + transform.up * 1, new Vector3(0.5f,0.5f,0.5f));

    }
}
