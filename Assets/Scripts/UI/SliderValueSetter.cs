using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderValueSetter : MonoBehaviour
{

    [SerializeField]
    private FloatData referenceValue;

    [SerializeField]
    private bool updateConstantly;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = referenceValue.Value;
    }

    private void Update() {
        if(updateConstantly)
        {
            slider.value = referenceValue.Value;
        }
    }
    // Update is called once per frame
    public void UpdateSliderValue()
    {
        slider.value = referenceValue.Value;
    }
}
