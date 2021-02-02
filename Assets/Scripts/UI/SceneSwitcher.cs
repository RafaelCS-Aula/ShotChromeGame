using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private bool QuitApplication = false;

    [Scene][HideIf("QuitApplication")][SerializeField]
    private string targetScene;

    [Button]
    public void SwitchToTargetScene()
    {
        if(QuitApplication)
            Application.Quit();
        else
            SceneManager.LoadScene(targetScene);

    } 
    
}
