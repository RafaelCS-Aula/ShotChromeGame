using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AreaofEffect))]
public class ShamanPowerBehaviour : LineOfSightAttack
{
    [SerializeField] private FloatData thunderPowerData;

    [SerializeField] private BoolData isLeeching;
    [SerializeField] private FloatVariable thunderLeechRate;

    [SerializeField] private FloatVariable buffPulseInterval;

    [SerializeField] private FloatVariable minimumThunder;

    private float _minimumThunderToBuff = 1.00f;
    private AreaofEffect _AoEComponent; 

    private float _healCooldownCr = 0;


    // Start is called before the first frame update
    protected override void  Start()
    {
        base.Start();
        _AoEComponent = GetComponent<AreaofEffect>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        
        if(thunderPowerData <= minimumThunder + 1 && _healCooldownCr < 0)
        {
            SendAoE();
            _healCooldownCr = buffPulseInterval;
        }

        _healCooldownCr -= Time.deltaTime;

        



    }

    protected override void Attack()
    {
        if(thunderPowerData < minimumThunder)
        {
            return;
        }
            

        isLeeching.SetValue(true);
        thunderPowerData.ApplyChange(-thunderLeechRate * Time.deltaTime);

    }

    private void SendAoE()
    {

            
            //print("heal aura");
            _AoEComponent.ApplyAoE(transform.position);
    
        

    }

    private void OnDestroy() {
        isLeeching.SetValue(false);
    }
    private void OnDisable() {
        isLeeching.SetValue(false);
    }
}
