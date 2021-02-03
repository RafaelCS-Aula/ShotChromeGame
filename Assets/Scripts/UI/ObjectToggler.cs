using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ObjectToggler : MonoBehaviour
{
    [ShowAssetPreview(64,64)][SerializeField]
    private GameObject[] affectedSceneObjects;

    /// <summary>
    /// Toggles the objects' active states
    /// </summary>
    [Button]
    public void ToggleState()
    {
    
        foreach(GameObject g in affectedSceneObjects)
        {
            bool newState = g.activeSelf ? false : true;
            g.SetActive(newState);
        }
    }
}
