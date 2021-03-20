using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpApplier : MonoBehaviour
{
   private Dictionary<string,PowerUp> activePowers = new Dictionary<string, PowerUp>();

   public bool IsPowerActive(string powerName) => activePowers.ContainsKey(powerName);

    public static PowerUpApplier Instance;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(Instance);
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
    public void ActivatePower(PowerUp power)
    {
        if(power.overwriteActive)
        {
            foreach(KeyValuePair<string,PowerUp> pu in activePowers)
            {
                pu.Value.isFinished = true;
                activePowers.Remove(pu.Key);
            }
        }

        power.Activate();
        activePowers.Add(power.powerName, power);
        
    }

    private void Update() 
    {
        foreach(KeyValuePair<string, PowerUp> pu in activePowers)
        {
            pu.Value.ApplyOverTime();
            if(pu.Value.isFinished)
                activePowers.Remove(pu.Key);
        }

    }

}
