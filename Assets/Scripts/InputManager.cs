using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public delegate void TwoDAxis(Vector2 direction);
    public delegate void KeyPress();

    [SerializeField] private KeycodeVariable jumpKey;
    [SerializeField] private KeycodeVariable shootKey;
    [SerializeField] private KeycodeVariable thunderKey;

    public TwoDAxis InpDirection;
    public KeyPress InpJump;
    public KeyPress InpShoot;
    public KeyPress InpThunder;

    //[SerializeField] private StringVariable  

    private Vector2 _cardinalInputs = new Vector2();
    // Update is called once per frame
    void Update()
    {
        _cardinalInputs.x = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        _cardinalInputs.y = Mathf.Round(Input.GetAxisRaw("Vertical"));
        InpDirection.Invoke(_cardinalInputs);

        if(Input.GetKey(jumpKey)) InpJump.Invoke();
        if(Input.GetKey(shootKey)) InpShoot.Invoke();
        if(Input.GetKey(thunderKey)) InpThunder.Invoke();



    }
}
