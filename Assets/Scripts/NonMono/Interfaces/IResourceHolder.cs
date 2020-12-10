using System;
using UnityEngine;

public interface IResourceHolder
{
    ResourceTypes Resource{get; set;}

    void AddResource(float amount);
    void ReduceResource(float amount);
    
}
