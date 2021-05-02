using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseControl : MonoBehaviour
{
    public static bool IsPaused;
    public bool StartPause;

    public bool stopTime;

    private void Awake() {
        if (StartPause)
        {
            IsPaused = false;
            TogglePause();
        }
        else
        {
            IsPaused = true;
            TogglePause();
        }

    }

    public void TogglePause()
    {
        //print(gameObject.name + "toggle pause");
        if(IsPaused)
        {
            if (stopTime) Time.timeScale = 1;
            IsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            if (stopTime) Time.timeScale = 0;
            IsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
