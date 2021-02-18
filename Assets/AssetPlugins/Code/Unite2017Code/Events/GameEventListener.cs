// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class GameEventListener : MonoBehaviour
{

    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]

    public bool useVoidResponse;
    public bool useVector3Response;
    public bool useFloatResponse;

    [ShowIf("useVoidResponse")]
    public UnityEvent Response;

    [ShowIf("useVector3Response")]
    public UnityEvent<Vector3> PositionResponse;
    
    [ShowIf("useFloatResponse")]
    public UnityEvent<float> DamageResponse;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();

    }
    public void OnPositionEventRaised(Vector3 eventPosition)
    {
        PositionResponse.Invoke(eventPosition);
    }

    public void OnDamageEventRaised(float damage)
    {
        DamageResponse.Invoke(damage);
    }
}