using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[RequireComponent(typeof(Collider))]
public class ProximityDetector : MonoBehaviour
{
    public UnityEvent OnDetection;
    private Collider _collider;

    [SerializeField]
    private LayerMask detectLayers;

    [SerializeField]
    private bool singleActivation = true;
    private bool _triggered;
    private void Awake() {
        _collider = GetComponent<Collider>();

        if(_collider != null)
            _collider.isTrigger = true;

        _triggered = false;
    }
    private void OnTriggerEnter(Collider other) {

        /*if(!gameObject.activeSelf)
            return;*/
        //print($"touched {other.gameObject.name} of layer {1<<other.gameObject.layer} but i only care about {detectLayers.value}");
        if(1<<other.gameObject.layer == detectLayers.value &&
            !_triggered)
        {
            //print("Player Entered Area");
            StartCoroutine(GetTriggered());
            _triggered = true;

            if (singleActivation) Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(1<<other.gameObject.layer == detectLayers.value &&
            !singleActivation)
            {
                _triggered = false;
            }

    }

private IEnumerator GetTriggered()
{
    yield return new WaitForFixedUpdate();
    OnDetection.Invoke();

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
