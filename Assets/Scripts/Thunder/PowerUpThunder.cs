using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class PowerUpThunder : MonoBehaviour
{
    public UnityEvent<Vector3, Vector3> OnPowerupSpawn;
    
    [SerializeField]
    private LayerMask impactLayer;
    [SerializeField]
    private FloatData _thunderPower;
    [SerializeField]
    private FloatData _maxThunderPower;

    [SerializeField][CurveRange(0,0,1,1)]
    private AnimationCurve strikeProbabilityCurve;

    [SerializeField]
    private FloatVariable minRangeFromCenter;
    [SerializeField]
    private FloatVariable maxRangeFromCenter;

    [SerializeField]
    private List<PowerUp> droppablePowers = new List<PowerUp>();


    private void Awake() {
        
    }
    public void TryStrike(Vector3 center)
    {

        Vector3 spawnPlace;
        float powerPercentage = _thunderPower/_maxThunderPower;
        //Debug.Log($"{_thunderPower.Value}");
        //powerPercentage = Mathf.Clamp01(powerPercentage);
        float myProbability = strikeProbabilityCurve.Evaluate(powerPercentage);
        //myProbability = Mathf.Clamp01(myProbability);
        
        float rngRoll = Random.Range(0.0f,1.0f);

        //Debug.Log($"Try Power strike: rng:{rngRoll.ToString("n3")} probability: {myProbability.ToString("n3")} power percentage: {powerPercentage.ToString("n3")}");
        if(rngRoll < myProbability)
        {
            //Find place to spawn powerup
            Vector3 newSpot = center + Random.insideUnitSphere * maxRangeFromCenter;
            if(newSpot.y < center.y)
            {
                float diff = center.y - newSpot.y;
                newSpot.y += diff*2;
            }
            Vector3 directionToSpot = (newSpot - center).normalized;

            RaycastHit hit;

            if(Physics.Raycast(center , directionToSpot,
            out hit,
            maxRangeFromCenter - minRangeFromCenter,
            impactLayer))
            {
                spawnPlace = hit.point;
               
            }
            else
            {
                spawnPlace = newSpot;
                
            }    
            StrikePowerUp(spawnPlace);
            OnPowerupSpawn.Invoke(center,spawnPlace);
        }
        //else
            //Debug.Log("No powerup spawn");

    }

    private void StrikePowerUp(Vector3 strikePoint)
    {
        PowerUp powerToSpawn = null;
        float probabilitySum = 0;
        
        // Use weighted chance to spawn one of the possible powerups
        for(int i = 0; i < droppablePowers.Count; i++)
        {
            probabilitySum += droppablePowers[i].baseChance;
        }

        for(int i = droppablePowers.Count - 1; i > -1; i--)
        {
            float rng = Random.Range(0.0f,probabilitySum);
            if(rng < droppablePowers[i].baseChance)
            {
                powerToSpawn = droppablePowers[i];
                break;
            }
            else
            {
                probabilitySum -= droppablePowers[i].baseChance;
            }
        }
        if(powerToSpawn == null)
            powerToSpawn = droppablePowers[0];

        powerToSpawn.SpawnPrefab(strikePoint);

    }
}
