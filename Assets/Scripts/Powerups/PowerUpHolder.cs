using UnityEngine;
using NaughtyAttributes;
[RequireComponent(typeof(Collider))]
public class PowerUpHolder : MonoBehaviour
{
    public PowerUp powerUp;

    private bool isCollected = false;

    [SerializeField]
    private float lifeTime = 0;

    [ShowNativeProperty]
    private bool isEternal => lifeTime == 0; 

    private float _timeAlive = 0;
    private void Update() {
        if(!isEternal)
        {
            _timeAlive += Time.deltaTime;
            if(_timeAlive >= lifeTime)
                Destroy(gameObject);
        }

        
    }

    private void OnTriggerEnter(Collider other) 
    {
        //Debug.Log("TriggerEnter!");
        //yield return null;
        if(other.gameObject.GetComponent<PowerUpCollector>() ||
        other.gameObject.GetComponentInChildren<PowerUpCollector>())
        {
            if(isCollected)
               return;
            if(!isCollected && !PowerUpApplier.Instance.IsPowerActive(powerUp.powerName))
            {
                isCollected = true;
                //Debug.Log($"{gameObject.name} Holder activation! {other.name}");
                
                powerUp.ApplyPowerup();
                Destroy(gameObject);
            }
            
        }
    }
}
