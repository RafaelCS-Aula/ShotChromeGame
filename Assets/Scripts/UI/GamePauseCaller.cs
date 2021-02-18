using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(GamePauseControl), typeof(ObjectToggler))]
public class GamePauseCaller : MonoBehaviour
{
    [SerializeField] private KeycodeVariable pauseKey;
    private GamePauseControl _pauseCtrl;

    [SerializeField][ReadOnly]
    private ObjectToggler _menuToggle;
    private void Awake() {
        _pauseCtrl = GetComponent<GamePauseControl>();
        _menuToggle = GetComponent<ObjectToggler>();
        //GamePauseControl.IsPaused = true;
        //_pauseCtrl.TogglePause();
    }

    private void Update() 
    {
        //print(GamePauseControl.IsPaused);
        if(Input.GetKeyDown(pauseKey.Value))
        {
            _pauseCtrl.TogglePause();
            _menuToggle.ToggleState();
        }
    }
}
