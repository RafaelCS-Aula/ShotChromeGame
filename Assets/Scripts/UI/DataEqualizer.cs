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

    [Button]
    public void Equalize()
    {
        for (int i = 0; i < _targetSet.Length; i++)
        {
            _targetSet[i].SetValue(_sourceSet[i].Value);
        }
    }
}
