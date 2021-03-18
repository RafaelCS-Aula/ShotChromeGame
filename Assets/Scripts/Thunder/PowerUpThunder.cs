using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PowerUpThunder : MonoBehaviour
{
    
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



    public void TryStrike(Vector3 center)
    {
        float powerPercentage = _thunderPower/_maxThunderPower;
        powerPercentage = Mathf.Clamp01(powerPercentage);
        float myProbability = strikeProbabilityCurve.Evaluate(powerPercentage);
        myProbability = Mathf.Clamp01(myProbability);

        float rngRoll = Random.Range(0,1);
        if(rngRoll <= myProbability)
        {
            //Find place to spawn powerup
            Vector3 newSpot = center + Random.insideUnitSphere * maxRangeFromCenter;

            Vector3 directionToSpot = (newSpot - center).normalized;

            RaycastHit hit;

            if(Physics.Raycast((center + directionToSpot * minRangeFromCenter), directionToSpot,
            out hit,
            maxRangeFromCenter - minRangeFromCenter,
            impactLayer))
            {
                StrikePowerUp(hit.point);
            }
            else
            {
                StrikePowerUp(newSpot);
            }    
        }

    }

    public void StrikePowerUp(Vector3 strikePoint)
    {
        PowerUp powerToSpawn;
        float probabilitySum = 0;
        
        // Use weighted chance to spawn one of the possible powerups
        for(int i = 0; i < droppablePowers.Count; i++)
        {
            probabilitySum += droppablePowers[i].baseChance.Value;
        }

        for(int i = droppablePowers.Count - 1; i > -1; i--)
        {
            float rng = Random.Range(0,probabilitySum);
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

    }
}
