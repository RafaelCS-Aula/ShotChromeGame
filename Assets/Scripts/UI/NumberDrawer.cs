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

    private float _displayVal;
    private float _displayValMax;

    private void OnEnable() 
    {
        _text = GetComponent<Text>();
        _slider = GetComponent<Slider>();

    }
    // Update is called once per frame
    void Update()
    {
        if(value != null && !useInt) 
            _displayVal = useInt ? valueInt.Value : value.Value;
        if(maxValue!= null && !useInt)
            _displayValMax = useInt ? maxValueInt.Value : maxValue.Value;
            
        if(isText && _text != null)
        {
            if(maxValue == null && value == null)
            _display = $"{valueName}";

            if(maxValue == null)
            _display = $"{valueName} {value.Value.ToString("n2")}";
            else
            _display = $"{valueName} {value.Value.ToString("n2")} / {maxValue.Value.ToString("n2")}";

            _text.text = _display;

        }
        
        if(isSlider && _slider != null && maxValue != null) 
        {
           
            _slider.value = value.Value;
            _slider.maxValue = maxValue.Value;
            _slider.minValue = 0;

        }
    }
}
