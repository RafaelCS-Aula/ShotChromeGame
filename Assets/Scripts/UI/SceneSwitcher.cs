using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    [SerializeField]
    private bool keyTriggered = false;

    [SerializeField]
    [ShowIf("keyTriggered")]
    private KeyCode triggerKey;

    [SerializeField]
    private bool QuitApplication = false;

    [Scene]
    [HideIf("QuitApplication")]
    [SerializeField]
    private string targetScene;

    private void Update()
    {
        if (!keyTriggered)
            return;
        if (Input.GetKeyDown(triggerKey))
            SwitchToTargetScene();
    }

    [Button]
    public void SwitchToTargetScene()
    {
#if UNITY_EDITOR
        if (Application.isEditor && QuitApplication) UnityEditor.EditorApplication.isPlaying = false;
#endif

        if (QuitApplication)
        {
            Application.Quit();
        }

        else
            SceneManager.LoadScene(targetScene);

    }

}
