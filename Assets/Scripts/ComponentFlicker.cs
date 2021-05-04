using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFlicker : MonoBehaviour
{
    [SerializeField] private float OnTime;
    

    public void CallDisableCR() => StartCoroutine(DisableAfterTime());
    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(OnTime);
        this.gameObject.SetActive(false);
    }
}
