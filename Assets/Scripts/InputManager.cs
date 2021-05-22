using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : MonoBehaviour
{

    public delegate void TwoDAxis(Vector2 direction);
    public delegate void KeyPress(bool keyDown);

    [SerializeField] private KeycodeVariable jumpKey;
    [SerializeField] private KeycodeVariable shootKey;
    [SerializeField] private KeycodeVariable thunderKey;

    [SerializeField] private KeycodeVariable bloodAmmoKey;

    public TwoDAxis InpDirection = delegate(Vector2 v){};
    public KeyPress InpJump = delegate(bool b){};
    public KeyPress InpShoot = delegate(bool b){};
    public KeyPress InpThunder = delegate(bool b){};

    public KeyPress InpBloodAmmo = delegate(bool b){};

    //[SerializeField] private StringVariable  

    private Vector2 _cardinalInputs = new Vector2();
    // Update is called once per frame
    void Update()
    {
        if(GamePauseControl.IsPaused)
            return;
        
        _cardinalInputs.x = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        _cardinalInputs.y = Mathf.Round(Input.GetAxisRaw("Vertical"));
        InpDirection.Invoke(_cardinalInputs);

        InpJump.Invoke(Input.GetKey(jumpKey));
        InpShoot.Invoke(Input.GetKeyDown(shootKey));
        InpThunder.Invoke(Input.GetKeyDown(thunderKey));
        InpBloodAmmo.Invoke(Input.GetKeyDown(bloodAmmoKey));



    }
}
