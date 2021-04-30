using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AreaofEffect))]
public class ShamanPowerBehaviour : LineOfSightAttack
{
    [SerializeField] private FloatData thunderPowerData;
    [SerializeField] private FloatVariable thunderLeechRate;

    [SerializeField] private FloatVariable buffPulseInterval;

    private float _minimumThunderToBuff = 1.00f;
    private AreaofEffect _AoEComponent; 



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

        if(thunderPowerData < 1)
        {
            StartCoroutine(SendAoE());

        }



    }

    protected override void Attack()
    {
        thunderPowerData.ApplyChange(-thunderLeechRate * Time.deltaTime);
    }

    private IEnumerator SendAoE()
    {
        yield return new WaitForSeconds(buffPulseInterval);
        _AoEComponent.ApplyAoE(transform.position);
        

    }
}
