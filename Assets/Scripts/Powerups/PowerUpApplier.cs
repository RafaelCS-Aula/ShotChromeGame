using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpApplier : MonoBehaviour
{
   private static HashSet<PowerUp> activePowers = new HashSet<PowerUp>();

    public void ActivatePower(PowerUp power)
    {
        if(power.overwriteActive)
        {
            foreach(PowerUp pu in activePowers)
            {
                pu.isFinished = true;
                activePowers.Remove(pu);
            }
        }

        power.Activate();
        activePowers.Add(power);
        
    }

    private void Update() 
    {
        foreach(PowerUp pu in activePowers)
        {
            pu.ApplyOverTime();
            if(pu.isFinished)
                activePowers.Remove(pu);
        }

    }

}
