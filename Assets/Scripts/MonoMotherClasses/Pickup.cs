using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Pickup<T>  : MonoBehaviour where T : ResourceType
{
    [SerializeField] protected LayerMask collectorLayer;
    [SerializeField] protected FloatData contentAmount;

    private void OnTriggerEnter(Collider other)
    {
        IResourceHolder<T>[] receivers = other.gameObject.GetComponents<IResourceHolder<T>>();
        foreach(IResourceHolder<T> r in receivers)
        {
            SendValue(r);
        }
    }

    protected virtual void SendValue(IResourceHolder<T> receiver) 
    {
        receiver.ReceiveResource(contentAmount);
        Destroy(gameObject);
    }
}
