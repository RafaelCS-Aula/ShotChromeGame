using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[System.Serializable]
public class StrikeEvent : UnityEvent<Vector3> { }

public class ThunderStrike : InputReceiverBase
{


    [Foldout("Events")]
    [SerializeField] private UnityEvent OnSummonEvent;
    [Foldout("Events")]
    [SerializeField] private StrikeEvent OnStrikeEvent;

    [Foldout("Positional Events")]
    [SerializeField] private UnityEvent<Vector3, Vector3> OnBlockedEventPos;

   // [Foldout("Positional Events")]
    // [SerializeField] private StrikeEvent OnBlockedEventTargetPos;

    [Foldout("Events")]
    [SerializeField] private UnityEvent OnBlockedEvent;

    [SerializeField] private FloatVariable summonToStrikeDelay;
    [SerializeField] private LayerMask impactLayer;

    [SerializeField] private LayerMask blockedByLayer;

    [SerializeField] private Transform originPoint;

    private const float _originHeight = 1500;

    [SerializeField] private FloatVariable summonCoolDown;

    private float _smnCooldownTimer;

    private void OnEnable()
    {

        RegisterForInput();

        if (originPoint == null)
            originPoint = transform;
    }

    protected override void RegisterForInput()
    {
        base.RegisterForInput();

        if (InputHolder == null) // There is no InputHolder
            return;

        if (UseInput)
            InputHolder.InpThunder += InputDown;
        else if (!UseInput)
            InputHolder.InpThunder -= InputDown;
    }

    private void InputDown(bool key) => _input = key;
    // Update is called once per frame
    void Update()
    {
        if (_input && UseInput && _smnCooldownTimer <= 0)
        {

            RaycastHit hitInfo;
            if (Physics.SphereCast(originPoint.position, 0.05f, originPoint.forward, out hitInfo, 1500, impactLayer))
            {
                Debug.DrawLine(originPoint.position, hitInfo.point, Color.green, 3);


                RaycastHit blockerInfo;
                if(Physics.Raycast(hitInfo.point,Vector3.up,out blockerInfo, 1500, blockedByLayer))
                {
                    
                    StartCoroutine(SummonBlocked(hitInfo.point,blockerInfo));
                    return;
                }
                
                StartCoroutine(SummonThunder(hitInfo.point));

            }
            _smnCooldownTimer = summonCoolDown;
        }

        _smnCooldownTimer -= Time.deltaTime;
    }


    public IEnumerator SummonThunder(Vector3 landingSpot)
    {
        
        //DEBUG///////////////////////////
        Debug.DrawLine(new Vector3(landingSpot.x + 1, landingSpot.y + 1, landingSpot.z), new Vector3(landingSpot.x - 1, landingSpot.y - 1, landingSpot.z), Color.red, 3);
        Debug.DrawLine(new Vector3(landingSpot.x + 1, landingSpot.y - 1, landingSpot.z), new Vector3(landingSpot.x - 1, landingSpot.y + 1, landingSpot.z), Color.red, 3);
        //////////////////////////////////////////
        
        
        if (summonToStrikeDelay > 0)
        {
            OnSummonEvent.Invoke();
        }
        yield return new WaitForSeconds(summonToStrikeDelay);

        Vector3 origin = landingSpot;
        //print("Landing place " +  origin);
        origin.y += _originHeight;
        //print("Origin" +  origin);

        RaycastHit strikeInfo;
        Physics.Raycast(origin, Vector3.down, out strikeInfo, _originHeight * 5, impactLayer);

        //print("StrikePoint " +  strikeInfo.point);

        OnStrikeEvent.Invoke(strikeInfo.point);


        ///DEBUG///////////////////////////////////////
        Debug.DrawLine(origin, strikeInfo.point, Color.yellow, 2);

        Debug.DrawLine(new Vector3(strikeInfo.point.x + 1, strikeInfo.point.y + 1, strikeInfo.point.z), new Vector3(strikeInfo.point.x - 1, strikeInfo.point.y - 1, strikeInfo.point.z), Color.yellow, 3);
        Debug.DrawLine(new Vector3(strikeInfo.point.x + 1, strikeInfo.point.y - 1, strikeInfo.point.z), new Vector3(strikeInfo.point.x - 1, strikeInfo.point.y + 1, strikeInfo.point.z), Color.yellow, 3);
        //////////////////////////////////////////

        yield return null;
    }

    public IEnumerator SummonBlocked(Vector3 originalLanding, RaycastHit blocker)
    {
        if (summonToStrikeDelay > 0)
        {
            OnSummonEvent.Invoke();
        }
        yield return new WaitForSeconds(summonToStrikeDelay);

        Debug.DrawLine(originalLanding, originalLanding + Vector3.up * _originHeight, Color.blue, 3);

        OnBlockedEvent.Invoke();
        OnBlockedEventPos.Invoke(originalLanding + Vector3.up * _originHeight,blocker.collider.gameObject.transform.position);

        
        yield return null;
    }

   
}

