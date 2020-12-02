using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThunderStrike : MonoBehaviour
{
    [SerializeField] private FloatVariable summonToStrikeDelay; 
    [SerializeField] private LayerMask impactLayer;
    [SerializeField] private UnityEvent OnSummonEvent;
    [SerializeField] private UnityEvent<Vector3> OnStrikeEvent;

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
            if(Physics.SphereCast(Camera.main.transform.position, 0.05f, Camera.main.transform.forward, out hitInfo, 1500, impactLayer))
            {
                Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.green, 3);
                StartCoroutine(SummonThunder(hitInfo.point));

            }

        }

    }

   /* public void SummonThunder(Vector3 landingSpot)
    {
        StartCoroutine(WaitUntilStrike());


    }*/

    public IEnumerator SummonThunder(Vector3 landingSpot)
    {
        Debug.DrawLine(new Vector3(landingSpot.x + 1, landingSpot.y + 1, landingSpot.z), new Vector3(landingSpot.x - 1, landingSpot.y - 1, landingSpot.z), Color.red, 3);
        Debug.DrawLine(new Vector3(landingSpot.x + 1, landingSpot.y - 1, landingSpot.z), new Vector3(landingSpot.x - 1, landingSpot.y + 1, landingSpot.z), Color.red, 3);

        if(summonToStrikeDelay > 0)
        {
            OnSummonEvent.Invoke();
        }
        yield return new WaitForSeconds(summonToStrikeDelay);

        Vector3 origin = landingSpot;
        origin.y += _originHeight;

        RaycastHit strikeInfo;
        Physics.Raycast(origin, Vector3.down,out strikeInfo,_originHeight, impactLayer);
        Debug.DrawRay(origin, Vector3.down,Color.yellow,2);

        Debug.DrawLine(new Vector3(strikeInfo.point.x + 1, strikeInfo.point.y + 1, strikeInfo.point.z), new Vector3(strikeInfo.point.x - 1, strikeInfo.point.y - 1, strikeInfo.point.z), Color.yellow, 3);
        Debug.DrawLine(new Vector3(strikeInfo.point.x + 1, strikeInfo.point.y - 1, strikeInfo.point.z), new Vector3(strikeInfo.point.x - 1, strikeInfo.point.y + 1, strikeInfo.point.z), Color.yellow, 3);

        OnStrikeEvent.Invoke(strikeInfo.point);
    }
    
}
