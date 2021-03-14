using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;


public class CombatEncounter : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnEncounterStart;
    [SerializeField]
    private UnityEvent OnEncounterComplete;
    
    [SerializeField][ReorderableList]
    private List<CombatWave> waves;

    private CombatWave _currentWave = null;
    private int _currentWaveIndex = 0;

    [SerializeField][ReadOnly]
    private bool _allWavesComplete = false;

    public void StartEncounter()
    {
        OnEncounterStart.Invoke();
        _currentWave = waves[0];
        _currentWave.BeginWave();
        _allWavesComplete = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(_currentWave == null || _allWavesComplete)
        {
            OnEncounterComplete.Invoke();
        }
        
        if(_currentWave.locked)
        {
            _currentWave.CheckLockConditions();
        }
        else
        {
            AdvanceWave();
        }

        for(int i = 0; i < _currentWaveIndex; i++)
        {
            waves[i].CheckCompletion();
        }

        foreach(CombatWave wave in waves)
        {
            if(!wave.complete)
                return;
        }
        _allWavesComplete = true;
    }

    private void AdvanceWave()
    {
        _currentWaveIndex++;
        if(_currentWaveIndex < waves.Count)
        {
            _currentWave = waves[_currentWaveIndex];
        }
        else
        {
            return;
        }
    }
}
