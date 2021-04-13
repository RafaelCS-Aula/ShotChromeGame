using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;
public class NumberDrawer : MonoBehaviour
{
    private Text _text;
    private TextMeshProUGUI _pText;
    private Slider _slider;
    private string _display;

    [HideIf("useInt")]
    [SerializeField] private FloatData value;
    [HideIf("useInt")]
    [SerializeField] private FloatData maxValue;
    
    [SerializeField] private string valueName;
    [SerializeField] private bool isSlider;
    [SerializeField] private bool isText;

    private float? _displayVal;
    private float? _displayValMax;

    private Component textComponent;

    private void OnEnable() 
    {
        _text = GetComponent<Text>();
        _slider = GetComponent<Slider>();
        _pText = GetComponent<TextMeshProUGUI>();
        
        if(_pText == null)
            textComponent = _text;
        else
            textComponent = _pText;

        if(isSlider && _slider != null && maxValue != null) 
        {
           
            _slider.value = value.Value;
            
            

        }
    }
    // Update is called once per frame
    void Update()
    {
        if(value != null ) 
            _displayVal = value.Value;
        if(maxValue!= null)
            _displayValMax = maxValue.Value;
            
        if(isText && (_text != null || _pText != null))
        {
          
               if(_displayValMax == null && _displayVal == null)
                _display = $"{valueName}";

                if(_displayValMax == null)
                _display = $"{valueName} {_displayVal.Value.ToString("n0")}";
                else
                _display = $"{valueName} {_displayVal.Value.ToString("n0")} / {_displayValMax.Value.ToString("n2")}";
               

           
           
            
        if(_pText == null)
            _text.text = _display;
        else
            _pText.text = _display;

        }
        

    }
}
