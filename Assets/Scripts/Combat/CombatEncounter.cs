using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[System.Serializable]
public class CombatWave
{
    public UnityEvent<Transform> StartWave;



}
public class CombatEncounter : MonoBehaviour
{
    [SerializeField][ReorderableList]
    private List<CombatWave> waves;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
