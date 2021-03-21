using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowerUpHolder : MonoBehaviour
{
    public PowerUp powerUp;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("TriggerEnter!");
        //yield return null;
        if(other.gameObject.GetComponent<PowerUpCollector>() ||
        other.gameObject.GetComponentInChildren<PowerUpCollector>())
        {
            if(isCollected)
               return;
            if(!isCollected && !PowerUpApplier.Instance.IsPowerActive(powerUp.powerName))
            {
                isCollected = true;
                Debug.Log($"{gameObject.name} Holder activation! {other.name}");
                
                powerUp.ApplyPowerup();
                Destroy(gameObject);
            }
            
        }

       

    }

}
