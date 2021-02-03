using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DataEqualizer : MonoBehaviour
{
    [ReorderableList][SerializeField]
    private FloatData[] _sourceSet;

    [ReorderableList][SerializeField]
    private FloatData[] _targetSet;

    [SerializeField]
    private bool _equalizeOnStart;

    private void Awake() 
    {

        if(!_equalizeOnStart)
            return;

        Equalize();

    }

    [Button]
    public void Equalize()
    {
        
        for (int i = 0; i < _targetSet.Length; i++)
        {
            //print($"({gameObject.name}): Set {_targetSet[i].name} to {_sourceSet[i].Value}, was {_targetSet[i].Value}");
            _targetSet[i].SetValue(_sourceSet[i].Value);
            
        }
    }
}
