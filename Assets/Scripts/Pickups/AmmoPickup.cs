using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AmmoPickup : Pickup<AmmoResource>
{
    [SerializeField] private FloatVariable TimeChargedAfterSpawn;
    private bool _poweredUp;
    private float _chargeTimer = 0;
    private void Start() {
        _chargeTimer = 0;
    }
    private void Update() 
    {
        if(_chargeTimer > TimeChargedAfterSpawn)
            _poweredUp = false;
        else
            _poweredUp = true;
        
        _chargeTimer += Time.deltaTime;
            
    

    }

    protected override void SendValue(IResourceHolder<AmmoResource> receiver)
    {
        if(!_poweredUp)
            base.SendValue(receiver);
        else
        (receiver as IResourceHolder<ThunderAmmoResource>)?.ReceiveResource(contentAmount);

        Destroy(gameObject);
        
    }

}
