using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;



//[CreateAssetMenu(menuName = "Combat/EncounterWave",fileName = "New Wave")]
[System.Serializable]
public class CombatWave //: ScriptableObject
{
    
    [SerializeField][ReadOnly][Tooltip("The encounter will not trigger the next wave while this is true")]
    public bool locked;
    
    [SerializeField][ReadOnly][Tooltip("The encounter will not be complete until all waves are marked as complete")]
    public bool complete;

    [SerializeField][ReadOnly][Tooltip("The encounter will not be complete until all waves are marked as complete")]
    public bool inProgress;


    [SerializeField]
    private SpawnerHolder _spawnGroups;


    [SerializeField][HorizontalLine][Expandable]
    private EnemyHolder jaguarHolder;
    [SerializeField][Expandable]
    private EnemyHolder flyerHolder;
    [SerializeField][Expandable]
    private EnemyHolder droneHolder;
    [SerializeField][Expandable]
    private EnemyHolder giantHolder;
    [SerializeField][Expandable]
    private EnemyHolder sandmanHolder;
    [SerializeField][Expandable]
    private EnemyHolder shamanHolder;
    [SerializeField][Expandable]
    private EnemyHolder specialHolder;
    
    

    [SerializeField][EnableIf("HasJaguarHolder"),AllowNesting]
    [HorizontalLine]
    private int _jaguars;
    [SerializeField][EnableIf("HasFlyerHolder"),AllowNesting]  
    private int _flyers;
    [SerializeField][EnableIf("HasDroneHolder"),AllowNesting] 
     private int _drones;
     [SerializeField][EnableIf("HasGiantHolder"),AllowNesting] 
     private int _giants;
     [SerializeField][EnableIf("HasSandmanHolder"),AllowNesting] 
     private int _sandmen;
     [SerializeField][EnableIf("HasShamanHolder"),AllowNesting]  
    private int _shamans;
    [SerializeField][EnableIf("HasSpecialHolder"),AllowNesting] 
     private int _specials;

    private bool HasJaguarHolder => jaguarHolder != null;
    private bool HasFlyerHolder => flyerHolder != null;
    private bool HasDroneHolder => droneHolder != null;
    private bool HasGiantHolder => giantHolder != null;
    private bool HasSandmanHolder => sandmanHolder != null;
    private bool HasShamanHolder => shamanHolder != null;
    private bool HasSpecialHolder => specialHolder != null;

    private bool UsesTimeUnlock => unlockCondition.HasFlag(WaveUnlockConditions.Time);
    private bool UsesKillUnlock => unlockCondition.HasFlag(WaveUnlockConditions.KillEnemies);

    private bool UsesKillTypeUnlock => unlockCondition.HasFlag(WaveUnlockConditions.KillEnemiesOfType);
    
   
    
    [SerializeField]
    private bool lockOnStart = true;    
    
    [SerializeField][ShowIf("lockOnStart"),AllowNesting]
    private WaveUnlockConditions unlockCondition;

    [SerializeField][MinValue(0), MaxValue(100)][ShowIf(EConditionOperator.And,"lockOnStart", "UsesKillUnlock"),AllowNesting]
    private float  totalKillPercentage;
    
    [SerializeField][ShowIf(EConditionOperator.And,"lockOnStart", "UsesKillTypeUnlock"),AllowNesting]
    private EnemyTypes TypeToGenocide; 

    [SerializeField][MinValue(0), MaxValue(100)][ShowIf(EConditionOperator.And,"lockOnStart", "UsesKillTypeUnlock"),AllowNesting]
    private float typeKillPercentage;

    [SerializeField][ShowIf(EConditionOperator.And,"lockOnStart", "UsesTimeUnlock"),AllowNesting]
    private float secondsToUnlock;


    private Dictionary<EnemyTypes, Stack<GameObject>> spawnedEnemies = new Dictionary<EnemyTypes, Stack<GameObject>>(); 
    private float _timeOnWaveStart;


    private Dictionary<EnemyHolder, int> _holdersAndCountsDict = new Dictionary<EnemyHolder, int>();

    private int _totalEnemies;
    private bool _allEnemiesInWaveSpawned = false;
    public void BeginWave()
    {

        if(lockOnStart)
            locked = true;
        
        if(jaguarHolder != null)
            _holdersAndCountsDict.Add(jaguarHolder, _jaguars);
        if(flyerHolder != null)    
            _holdersAndCountsDict.Add(flyerHolder, _flyers);
        if(droneHolder != null)
        _holdersAndCountsDict.Add(droneHolder, _drones);
        if(droneHolder != null)
            _holdersAndCountsDict.Add(giantHolder, _giants);
        if(sandmanHolder != null)
            _holdersAndCountsDict.Add(sandmanHolder, _sandmen);
        if(shamanHolder != null)
            _holdersAndCountsDict.Add(shamanHolder, _shamans);
        if(specialHolder != null)
            _holdersAndCountsDict.Add(specialHolder, _specials);


        _timeOnWaveStart = Time.realtimeSinceStartup;
        spawnedEnemies = new Dictionary<EnemyTypes, Stack<GameObject>>();


        Stack<GameObject> spawnedStack = new Stack<GameObject>();
        foreach(KeyValuePair<EnemyHolder, int> HoldersCounts in _holdersAndCountsDict)
        {
            spawnedEnemies.Add(HoldersCounts.Key.enemyType, _spawnGroups.PopulateType(HoldersCounts.Key, HoldersCounts.Value, this));
            
            if(spawnedEnemies.TryGetValue(HoldersCounts.Key.enemyType, out spawnedStack))
                _totalEnemies += spawnedStack.Count;
        }
        inProgress = true;
        Debug.Log($"Total Enemies: {_totalEnemies}");

        _allEnemiesInWaveSpawned = CheckIfAllSpawned();
    }


    public void CheckLockConditions()
    {
        if(!locked)
            return;

        
        // Check the time condition
        if(unlockCondition.HasFlag(WaveUnlockConditions.Time))
        {
            float currentTime = Time.realtimeSinceStartup;
            float timeDelta = currentTime - _timeOnWaveStart;

            if(timeDelta > secondsToUnlock)
            {
                locked = false;
                return;
            }
        }

        if(!_allEnemiesInWaveSpawned)
        {
            _allEnemiesInWaveSpawned = CheckIfAllSpawned();
        }

       if(unlockCondition.HasFlag(WaveUnlockConditions.KillEnemies))
       {
           // Only start checking once all enemies of this wave have spawned
           if(!_allEnemiesInWaveSpawned)
            return;
           int totalLiving = 0;
            foreach(KeyValuePair<EnemyTypes, Stack<GameObject>> kv in spawnedEnemies)
            {
                
                foreach(GameObject go in kv.Value)
                {
                    if(go != null)
                    {
                        if(go.activeSelf)
                        {
                            totalLiving++;
                            
                        }
                    }
                }
            }
            
            float percentage = (totalLiving * 100)/_totalEnemies;
            Debug.Log(totalLiving);
            Debug.Log($"Percentage: {percentage}");
            if(percentage <= totalKillPercentage)
            {

                locked = false;
                return;
            }
        }

        if(unlockCondition.HasFlag(WaveUnlockConditions.KillEnemiesOfType))
        {
            // Only start checking once all enemies of this wave have spawned
            if(!_allEnemiesInWaveSpawned)
                return;

            int typeTotal = 0;
            int typeAlive = 0;
            Stack<GameObject> typeStack;
            foreach(KeyValuePair<EnemyHolder, int> kv in _holdersAndCountsDict)
            {
                if(kv.Key.enemyType == TypeToGenocide)
                {
                    typeTotal = kv.Value;
                    if(spawnedEnemies.TryGetValue(TypeToGenocide, out typeStack))
                    {
                        foreach(GameObject enemyObject in typeStack)
                        {
                            if(enemyObject != null)
                            {
                                if(enemyObject.activeSelf)
                                {
                                    typeAlive++;

                                    
                                    
                                }
                            }
                        }

                    }
                }
                float percentage = (typeAlive * 100) / typeTotal;
                if(percentage <= typeKillPercentage)
                {
                    locked = false;
                    return;
                }
            }

        }
       
        
    }
    
    public void CheckCompletion()
    {
        foreach(KeyValuePair<EnemyTypes, Stack<GameObject>> kv in spawnedEnemies)
        {
            foreach(GameObject go in kv.Value)
            {
                if(go != null)
                {
                    if(go.activeSelf)
                    {
                        return;
                    }
                }
            }
        }
        complete = true;
        inProgress = false;


    }
    

    public void AddLateSpawnedEnemy(EnemyTypes enemyType, GameObject enemyObj)
    {
        Stack<GameObject> dummyStack;
        if(spawnedEnemies.TryGetValue(enemyType,out dummyStack))
            dummyStack.Push(enemyObj);
    }
    
    // Check if the spawned count is the same as the count specified by the user
    private bool CheckIfAllSpawned()
    {
        foreach(KeyValuePair<EnemyHolder, int> hkv in _holdersAndCountsDict)
        {
            if(spawnedEnemies[hkv.Key.enemyType].Count < hkv.Value)
            {
                return false;

            }
            else
                return true;
        }
        return true;
    }

}
