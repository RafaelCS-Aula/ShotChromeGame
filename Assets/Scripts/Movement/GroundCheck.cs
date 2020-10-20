using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    private Vector3 feetPosition;

    [SerializeField]
    private float feetRadius;
    private IUseGround[] _groundRequesters;

    // Start is called before the first frame update
    void Awake()
    {
        _groundRequesters = GetComponents<IUseGround>();
        print(_groundRequesters.Length);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < _groundRequesters.Length; i++)
        {
            _groundRequesters[i].touchingGround =
                CheckGround(_groundRequesters[i].CollisionLayer);        
           // print(_groundRequesters[i].touchingGround);
        }
    }

    private bool CheckGround(int layer)
    {
        Vector3 pos = feetPosition + transform.localPosition;

        // Bit shift the layers to the desired one
        Collider[] collisions = 
            Physics.OverlapSphere(pos, feetRadius, 1 << layer);
        return collisions.Length > 0 ;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(feetPosition + transform.localPosition, feetRadius);

    }


}
