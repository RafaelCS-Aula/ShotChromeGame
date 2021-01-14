using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AmmoPickup : Pickup<AmmoResource>
{
    [SerializeField] private FloatVariable chargeTime;
    [SerializeField] private BoolData poweredUp;
    private float _chargeTimer = 0;

    private void Update() 
    {
        if(_chargeTimer > chargeTime)
            return;
        
        _chargeTimer += Time.deltaTime;
        poweredUp.SetValue(chargeTime < _chargeTimer);    
    

    }

    protected override void SendValue(IResourceHolder<AmmoResource> receiver)
    {
        if(!poweredUp)
            base.SendValue(receiver);
        else
        (receiver as IResourceHolder<ThunderAmmoResource>)?.ReceiveResource(contentAmount);

        Destroy(gameObject);
        
    }

}
