using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WeaponScreenPositionSwitcher : MonoBehaviour
{

    [SerializeField] private Vector3Variable rightSidePosition;
    [SerializeField] private Vector3Variable centerPosition;

    [SerializeField] private BoolVariable centerWeapon;

    private Vector3 _currentWeaponPosition;
 


    // Update is called once per frame
    void Update()
    {
        Vector3 sPos = _currentWeaponPosition;
        if(centerWeapon)
        {
            _currentWeaponPosition = centerPosition;
        }
        else
        {
            _currentWeaponPosition = rightSidePosition;
        }
        Vector3 endPos = _currentWeaponPosition;

        if(sPos != endPos)
            transform.localPosition = _currentWeaponPosition;

        
    }

    public void MatchCurrentPosition() => _currentWeaponPosition = transform.position;

    
}
