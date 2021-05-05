using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private SceneSwitcher sS;
    public KeyCode skipKey;
    public GameObject skipNotify;

    private float _skipTimer = 2.00f;

    private float _currentSkipTImer;
    // Start is called before the first frame update
    void Start()
    {
        sS = GetComponent<SceneSwitcher>();
        StartCoroutine(Delay());
    }


    // Update is called once per frame
    private void Update() {
        if(Input.GetKeyDown(skipKey) && _currentSkipTImer > 0)
        {
             sS.SwitchToTargetScene();
            
        }
        else if(Input.GetKeyDown(skipKey) && _currentSkipTImer <= 0)
        {
           _currentSkipTImer = _skipTimer;
        }

        _currentSkipTImer -= Time.deltaTime;

        if(_currentSkipTImer <= 0)
        {
            
            skipNotify.SetActive(false);
        }
        else
            skipNotify.SetActive(true);
            
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(19);
        sS.SwitchToTargetScene();
    }
}
