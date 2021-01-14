using System;
using UnityEngine;

public interface IResourceHolder<T> where T : ResourceType
{
    
    void ReceiveResource(float amount);
    
}
