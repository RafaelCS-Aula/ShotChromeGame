using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[RequireComponent(typeof(SphereCollider))]
public class ProximityDetector : MonoBehaviour
{
    public UnityEvent OnDetection;
    private SphereCollider _collider;

    [SerializeField]
    private LayerMask detectLayers;

    private void Awake() {
        _collider = GetComponent<SphereCollider>();

        if(_collider != null)
            _collider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == detectLayers)
        {
            OnDetection.Invoke();
        }
    }

    private void OnDrawGizmos() {
        
        
        

        Handles.color = Color.green;
        Handles.Label(transform.position + transform.up * 1.4f,"Proximity Detector");
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
       // Gizmos.DrawWireSphere(transform.position, _collider.radius);
    }
}
