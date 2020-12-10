using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StrikeEvent : UnityEvent<Vector3>{}

public class ThunderStrike : InputReceiverBase
{
    [SerializeField] private FloatVariable summonToStrikeDelay; 
    [SerializeField] private LayerMask impactLayer;
    [SerializeField] private UnityEvent OnSummonEvent;
    [SerializeField] private StrikeEvent OnStrikeEvent;

    [SerializeField] private Transform originPoint;
    public KeyCode summonKey;
    
    private const float _originHeight = 1500;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(summonKey))
        {
            RaycastHit hitInfo;
            if(Physics.SphereCast(originPoint.position, 0.05f, originPoint.forward, out hitInfo, 1500, impactLayer))
            {
                Debug.DrawLine(originPoint.position, hitInfo.point, Color.green, 3);
                StartCoroutine(SummonThunder(hitInfo.point));

            }

        }

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
    
}
