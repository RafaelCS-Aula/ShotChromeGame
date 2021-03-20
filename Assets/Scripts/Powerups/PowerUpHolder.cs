using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowerUpHolder : MonoBehaviour
{
    public PowerUp powerUp;

    private void OnTriggerEnter(Collider other) 
    {

        if(other.gameObject.GetComponent<PowerUpCollector>() ||
        other.gameObject.GetComponentInChildren<PowerUpCollector>())
        {
            powerUp.ApplyPowerup();
        }

    }

}
