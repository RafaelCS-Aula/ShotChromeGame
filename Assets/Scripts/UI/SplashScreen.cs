using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private SceneSwitcher sS;
    // Start is called before the first frame update
    void Start()
    {
        sS = GetComponent<SceneSwitcher>();
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(19);
        sS.SwitchToTargetScene();
    }
}
