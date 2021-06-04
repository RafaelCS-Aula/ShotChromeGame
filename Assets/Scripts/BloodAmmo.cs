using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class BloodAmmo : InputReceiverBase
{
    [Foldout("Events")]
    public UnityEvent<float> OnBloodForAmmo_BloodAmount;
    [Foldout("Events")]
    public UnityEvent<float> OnBloodForAmmo_AmmoAmount;
    [Foldout("Events")]
    public UnityEvent OnBloodForAmmo;


    [SerializeField] private FloatVariable useCoolDown;
    [SerializeField] private FloatVariable minimumHealth;
    [SerializeField] private FloatData HealthData;
    [SerializeField] private FloatData AmmoData;

    [ShowNativeProperty]
    private float receivedAmmoOnUse => HPConsumedOnUse.Value * AmmoPerHealth.Value;

    
    [Tooltip("Ratio of how much Ammo is given per each unit of health")]
    public FloatVariable AmmoPerHealth;

    [Tooltip("How much health is taken on use of ability")]
    public FloatVariable HPConsumedOnUse;

    private float _coolDownTimer = 0;

    protected override void RegisterForInput()
    {
        base.RegisterForInput();
        if(UseInput)
            InputHolder.InpBloodAmmo += InputDown;
        else if(!UseInput)
            InputHolder.InpBloodAmmo -= InputDown;
    }

    private void InputDown(bool key) => _input = key;

    private void Awake() {
        RegisterForInput();
    }

    // Update is called once per frame
    void Update()
    {
        //print(_input);
        if(_input && _coolDownTimer > useCoolDown)
        {
            TradeBloodForAmmo(HPConsumedOnUse);
            _coolDownTimer = 0;
        }

        _coolDownTimer += Time.deltaTime;
    }

    private void TradeBloodForAmmo(float amountOfBlood)
    {
        if(HealthData.Value <= minimumHealth)
            return;

        float currentAmmo = AmmoData.Value;

        float usedBlood = (HealthData.Value - amountOfBlood < minimumHealth) ? HealthData.Value - minimumHealth : amountOfBlood;

        

        int gainedAmmo = (int)usedBlood * AmmoPerHealth;

        AmmoData.ApplyChange(usedBlood * AmmoPerHealth);

        // If there was a change in the ammo amount then call
        // events and deduct health
        if(currentAmmo != AmmoData.Value)
        {
            HealthData.ApplyChange(-usedBlood);
            OnBloodForAmmo.Invoke();
            OnBloodForAmmo_AmmoAmount.Invoke(gainedAmmo);
            OnBloodForAmmo_BloodAmount.Invoke(usedBlood);
        }

        

    }
}
