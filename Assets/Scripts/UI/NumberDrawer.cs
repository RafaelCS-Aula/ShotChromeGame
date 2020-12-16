using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
public class NumberDrawer : MonoBehaviour
{
    private Text _text;
    private Slider _slider;
    private string _display;

    [HideIf("useInt")]
    [SerializeField] private FloatData value;
    [HideIf("useInt")]
    [SerializeField] private FloatData maxValue;

    [ShowIf("useInt")]
    [SerializeField] private IntData valueInt;

    [ShowIf("useInt")]
    [SerializeField] private IntData maxValueInt;
    
    [SerializeField] private string valueName;
    [SerializeField] private bool isSlider;
    [SerializeField] private bool isText;

    [SerializeField] private bool useInt;

    private void OnEnable() 
    {
        _text = GetComponent<Text>();
        _slider = GetComponent<Slider>();

    }
    // Update is called once per frame
    void Update()
    {
        float displayval = useInt ? valueInt.Value : value.Value;
        float displaymaxVal = useInt ? maxValueInt.Value : maxValue.Value;


        if(isText && _text != null)
        {
            if(maxValue == null)
            _display = $"{valueName}: {value.Value}";
            else
            _display = $"{valueName}: {value.Value} / {maxValue.Value}";

            _text.text = _display;

        }
        
        if(isSlider && _slider != null && maxValue != null) 
        {
            print("boing");
            _slider.value = value.Value;
            _slider.maxValue = maxValue.Value;
            _slider.minValue = 0;

        }
    }
}
