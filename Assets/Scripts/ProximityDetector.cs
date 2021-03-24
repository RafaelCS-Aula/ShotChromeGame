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

    [SerializeField]
    private bool singleActivation = true;
    private bool _triggered;
    private void Awake() {
        _collider = GetComponent<SphereCollider>();

        if(_collider != null)
            _collider.isTrigger = true;

        _triggered = false;
    }
    private void OnTriggerEnter(Collider other) {

        /*if(!gameObject.activeSelf)
            return;*/
        print($"touched {other.gameObject.name} of layer {1<<other.gameObject.layer} but i only care about {detectLayers.value}");
        if(1<<other.gameObject.layer == detectLayers.value &&
            !_triggered)
        {
            print("Player Entered Area");
            OnDetection.Invoke();
            _triggered = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(1<<other.gameObject.layer == detectLayers.value &&
            !singleActivation)
            {
                _triggered = false;
            }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        
        
        

        Handles.color = Color.green;
        Handles.Label(transform.position + transform.up * 1.4f,"Proximity Detector");
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
       // Gizmos.DrawWireSphere(transform.position, _collider.radius);
    }
#endif
}
