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
    private List<CombatWave> waves = new List<CombatWave>();

    private CombatWave _currentWave = null;
    private int _currentWaveIndex = 0;

    [SerializeField][ReadOnly]
    private bool _allWavesComplete = false;

    [SerializeField][ReadOnly]
    private bool ongoing = false;

    [Button]
    public void StartEncounter()
    {
        _currentWaveIndex = 0;
        OnEncounterStart.Invoke();
        _currentWave = waves[_currentWaveIndex];
        _currentWave.BeginWave();
        _allWavesComplete = false;
        ongoing = true;
    }

    private void Start() {
        print(waves.Count);
    }
    // Update is called once per frame
    void Update()
    {
        if(!ongoing)
            return;
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

    

        foreach(CombatWave wave in waves)
        {
            if(!wave.locked)
                wave.CheckCompletion();
            if(!wave.complete)
                return;
        }
        _allWavesComplete = true;
        ongoing = false;
    }

    private void AdvanceWave()
    {
        _currentWaveIndex++;
        if(_currentWaveIndex < waves.Count)
        {
            _currentWave = waves[_currentWaveIndex];
            _currentWave.BeginWave();
        }
        else
        {
            return;
        }
    }
}
