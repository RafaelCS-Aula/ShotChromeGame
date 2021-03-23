using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectToggler))]
public class SuperAmmoChecker : MonoBehaviour
{
    [SerializeField]
    private BoolData superChargeStatus;

    [SerializeField]
    private Color superChargeColor;

    private Color _normalColor;

    [SerializeField] private TMPro.TMP_Text SuperCounter;
    // Update is called once per frame
    private void Start() {
        _normalColor = SuperCounter.color;
    }
    void Update()
    {
        if(SuperCounter == null)
            return;

        if(superChargeStatus)
            SuperCounter.color = superChargeColor;
        else
            SuperCounter.color = _normalColor;

    }
}
