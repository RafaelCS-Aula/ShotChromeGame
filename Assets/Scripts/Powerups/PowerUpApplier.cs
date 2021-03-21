using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpApplier : MonoBehaviour
{
   private Dictionary<string,PowerUp> activePowers = new Dictionary<string, PowerUp>();

    private List<string> finishedPowers = new List<string>(); 
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
            }
            activePowers.Clear();
        }
        Debug.Log("Applier Activation!");
        power.Activate();
     
        if(!power.isFinished)
            activePowers.Add(power.powerName, power);
        
    }

    private void Update() 
    {
        foreach(KeyValuePair<string, PowerUp> pu in activePowers)
        {
            pu.Value.ApplyOverTime();
           if(pu.Value.isFinished)
           {
               
               finishedPowers.Add(pu.Key);
           }
                
            
        }

        foreach(string key in finishedPowers)
        {
            if(activePowers.ContainsKey(key))
            {
                Debug.Log($"Removing {key}");
                activePowers.Remove(key);
            }
        }

        finishedPowers.Clear();

    }

    

}
