using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectToggler))]
public class SuperAmmoChecker : MonoBehaviour
{
    [SerializeField]
    private BoolData superChargeStatus;

    [SerializeField] private GameObject SuperCounter;
    // Update is called once per frame
    void Update()
    {
        if(SuperCounter == null)
            return;

        if(superChargeStatus)
            SuperCounter.SetActive(true);
        else
            SuperCounter.SetActive(false);

    }
}
