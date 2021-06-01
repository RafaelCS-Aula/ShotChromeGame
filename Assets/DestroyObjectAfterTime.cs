using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterTime : MonoBehaviour
{
    [SerializeField] private FloatVariable LifeTime;
    
    private void OnEnable() 
    {

        
        StartCoroutine(CountDown());
    }

    public IEnumerator CountDown()
    {
        if(LifeTime == null)
            yield return null;
        yield return new WaitForSecondsRealtime(LifeTime.Value);
    }
}
