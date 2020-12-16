using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private KeycodeVariable reloadSceneKey;
    [SerializeField] private const float keyHoldTime = 2;

    [Scene]
    [SerializeField] private string sceneToReload;
    private float _timer;

    void Awake() => _timer = 0;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(reloadSceneKey))
        {
            _timer += Time.deltaTime;
            if(_timer >= keyHoldTime)
                SceneManager.LoadScene(sceneToReload);

        }
        else
        {
            _timer = 0;
        }
    }


}
