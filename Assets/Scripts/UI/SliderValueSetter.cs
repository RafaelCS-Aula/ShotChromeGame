using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderValueSetter : MonoBehaviour
{

    [SerializeField]
    private FloatData referenceValue;

    private Slider slider;
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void UpdateSliderValue()
    {
        slider.value = referenceValue.Value;
    }
}
