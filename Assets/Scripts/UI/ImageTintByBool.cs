using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTintByBool : MonoBehaviour
{
    [SerializeField]
    private BoolData condition;

    [SerializeField]
    private bool invertCondition = false;

    private bool _useCondition;


    [SerializeField]
    private Image image;

    private Color _regularColor;

    [SerializeField] private Color trueConditionColor;
    [SerializeField] private Color falseConditionColor;

    // Start is called before the first frame update
    void Start()
    {
        _regularColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(invertCondition)
            _useCondition = !condition;
        else
            _useCondition = condition;

        if(_useCondition)
        {
            image.color = _regularColor * trueConditionColor;
        }
        else
            image.color = _regularColor * falseConditionColor;
    }
}
