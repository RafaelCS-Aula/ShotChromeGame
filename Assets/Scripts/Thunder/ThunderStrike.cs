using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[System.Serializable]
public class StrikeEvent : UnityEvent<Vector3>{}

public class ThunderStrike : InputReceiverBase
{
    

    [Foldout("Events")]
    [SerializeField] private UnityEvent OnSummonEvent;
    [Foldout("Events")]
    [SerializeField] private StrikeEvent OnStrikeEvent;

    [SerializeField] private FloatVariable summonToStrikeDelay; 
    [SerializeField] private LayerMask impactLayer;

    [SerializeField] private Transform originPoint;

    private const float _originHeight = 1500;

    [SerializeField] private FloatVariable summonCoolDown;

    private float _smnCooldownTimer;

    private void OnEnable()
    {
      
      RegisterForInput();

      if(originPoint == null)
        originPoint = transform;
    }

    protected override void RegisterForInput()
    {
        base.RegisterForInput();

        if(InputHolder == null) // There is no InputHolder
            return;
        
        if(UseInput)
            InputHolder.InpThunder += InputDown;
        else if(!UseInput)
            InputHolder.InpThunder -= InputDown;
    }

    private void InputDown(bool key) => _input = key;
    // Update is called once per frame
    void Update()
    {        
        if(_input && UseInput && _smnCooldownTimer <= 0)
        {
            
            RaycastHit hitInfo;
            if(Physics.SphereCast(originPoint.position, 0.05f, originPoint.forward, out hitInfo, 1500, impactLayer))
            {
                Debug.DrawLine(originPoint.position, hitInfo.point, Color.green, 3);


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
        
        if(summonToStrikeDelay > 0)
        {
            OnSummonEvent.Invoke();
        }
        yield return new WaitForSeconds(summonToStrikeDelay);

        Vector3 origin = landingSpot;
        origin.y += _originHeight;

        RaycastHit strikeInfo;
        Physics.Raycast(origin, Vector3.down,out strikeInfo,_originHeight, impactLayer);

        OnStrikeEvent.Invoke(strikeInfo.point);


        ///DEBUG///////////////////////////////////////
        Debug.DrawLine(origin, strikeInfo.point,Color.yellow,2);

        Debug.DrawLine(new Vector3(strikeInfo.point.x + 1, strikeInfo.point.y + 1, strikeInfo.point.z), new Vector3(strikeInfo.point.x - 1, strikeInfo.point.y - 1, strikeInfo.point.z), Color.yellow, 3);
        Debug.DrawLine(new Vector3(strikeInfo.point.x + 1, strikeInfo.point.y - 1, strikeInfo.point.z), new Vector3(strikeInfo.point.x - 1, strikeInfo.point.y + 1, strikeInfo.point.z), Color.yellow, 3);
         //////////////////////////////////////////

        
    }

    [Button]
    private void TestSummon()
    {
            RaycastHit hitInfo;
            if(Physics.SphereCast(originPoint.position, 0.05f, originPoint.forward, out hitInfo, 1500, impactLayer))
            {
                Debug.DrawLine(originPoint.position, hitInfo.point, Color.green, 3);

                

                if(summonToStrikeDelay > 0)
                {
                    OnSummonEvent.Invoke();
                }
                

            Vector3 origin = hitInfo.point;
            origin.y += _originHeight;

             //DEBUG///////////////////////////
        Debug.DrawLine(new Vector3(hitInfo.point.x + 1, hitInfo.point.y + 1, hitInfo.point.z), new Vector3(hitInfo.point.x - 1, hitInfo.point.y - 1, hitInfo.point.z), Color.red, 3);
        Debug.DrawLine(new Vector3(hitInfo.point.x + 1, hitInfo.point.y - 1, hitInfo.point.z), new Vector3(hitInfo.point.x - 1, hitInfo.point.y + 1, hitInfo.point.z), Color.red, 3);
        //////////////////////////////////////////

            RaycastHit strikeInfo;
            Physics.Raycast(origin, Vector3.down,out strikeInfo,_originHeight, impactLayer);

            OnStrikeEvent.Invoke(strikeInfo.point);

            ///DEBUG///////////////////////////////////////
        Debug.DrawLine(origin, strikeInfo.point,Color.yellow,2);

        Debug.DrawLine(new Vector3(strikeInfo.point.x + 1, strikeInfo.point.y + 1, strikeInfo.point.z), new Vector3(strikeInfo.point.x - 1, strikeInfo.point.y - 1, strikeInfo.point.z), Color.yellow, 3);
        Debug.DrawLine(new Vector3(strikeInfo.point.x + 1, strikeInfo.point.y - 1, strikeInfo.point.z), new Vector3(strikeInfo.point.x - 1, strikeInfo.point.y + 1, strikeInfo.point.z), Color.yellow, 3);
         //////////////////////////////////////////
            }    

            
        
    }
        
    
}
