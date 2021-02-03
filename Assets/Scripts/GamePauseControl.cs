﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseControl : MonoBehaviour
{
    public static bool IsPaused;
    public bool StartPause;

    private void OnEnable() {
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
        print("toggling pause");
        if(IsPaused)
        {
            Time.timeScale = 1;
            IsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0;
            IsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
