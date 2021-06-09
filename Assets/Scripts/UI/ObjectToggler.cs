using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ObjectToggler : MonoBehaviour
{
    [SerializeField] private float toggleDelay;

    [ShowAssetPreview(64,64), SerializeField]
    private GameObject[] affectedSceneObjects;
    [ShowAssetPreview(64,64), SerializeField]
    private GameObject[] enabledObjects;
    [ShowAssetPreview(64,64), SerializeField]
    private GameObject[] disabledObjects;

    /// <summary>
    /// Toggles the objects' active states
    /// </summary>
    [Button]
    public void ToggleState()
    {
        StartCoroutine(ToggleStateWithDelay());
    }

    private IEnumerator ToggleStateWithDelay()
    {
        yield return new WaitForSeconds(toggleDelay);

        foreach (GameObject g in affectedSceneObjects)
        {
            bool newState = g.activeSelf ? false : true;
            g.SetActive(newState);
        }

        foreach (GameObject g in enabledObjects)
        {
            g.SetActive(true);
        }
        
        foreach (GameObject g in disabledObjects)
        {
            g.SetActive(false);
        }
    }
}
