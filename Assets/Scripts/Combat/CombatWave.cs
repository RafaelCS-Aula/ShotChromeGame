using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;



[CreateAssetMenu(menuName = "Combat/EncounterWave",fileName = "New Wave")]
public class CombatWave : ScriptableObject
{

    [ReadOnly][Tooltip("The encounter will not trigger the next wave while this is true")]
    public bool locked;
    [ReadOnly][Tooltip("The encounter will not be complete until all waves are marked as complete")]
    public bool complete;


    [SerializeField]
    private SpawnerHolder _spawnGroups;

    [SerializeField][HorizontalLine]
    private EnemyHolder jaguarHolder;
    [SerializeField]
    private EnemyHolder flyerHolder;
    [SerializeField]
    private EnemyHolder droneHolder;
    [SerializeField]
    private EnemyHolder giantHolder;
    [SerializeField]
    private EnemyHolder sandmanHolder;
    [SerializeField]
    private EnemyHolder shamanHolder;
    [SerializeField]
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

    private bool HasJaguarHolder() => jaguarHolder != null;
    private bool HasFlyerHolder() => flyerHolder != null;
    private bool HasDroneHolder() => droneHolder != null;
    private bool HasGiantHolder() => giantHolder != null;
    private bool HasSandmanHolder() => sandmanHolder != null;
    private bool HasShamanHolder() => shamanHolder != null;
    private bool HasSpecialHolder() => specialHolder != null;

    private bool UsesTimeUnlock() => unlockCondition.HasFlag(WaveUnlockConditions.Time);
    private bool UsesKillUnlock() => unlockCondition.HasFlag(WaveUnlockConditions.KillEnemies);

    private bool UsesKillTypeUnlock() => unlockCondition.HasFlag(WaveUnlockConditions.KillEnemiesOfType);
    
   
    
    [SerializeField]
    private bool lockOnStart = true;    
    
    [SerializeField][ShowIf("lockOnStart")]
    private WaveUnlockConditions unlockCondition;

    [SerializeField][MinValue(0), MaxValue(100)][ShowIf(EConditionOperator.And,"lockOnStart", "UsesKillUnlock")]
    private float  totalKillPercentage;
    
    [SerializeField][ShowIf(EConditionOperator.And,"lockOnStart", "UsesKillTypeUnlock")]
    private EnemyTypes TypeToGenocide; 

    [SerializeField][MinValue(0), MaxValue(100)][ShowIf(EConditionOperator.And,"lockOnStart", "UsesKillTypeUnlock")]
    private float typeKillPercentage;

    [SerializeField][ShowIf(EConditionOperator.And,"lockOnStart", "UsesTimeUnlock")]
    private float secondsToUnlock;


    private Dictionary<EnemyTypes, Stack<GameObject>> spawnedEnemies; 
    private float _timeOnWaveStart;

    //private int[] _enemyCountArray = 
    //    new int[Enum.GetNames(typeof(EnemyTypes)).Length];
   // private EnemyHolder[] _holdersArray =
   //     new EnemyHolder[Enum.GetNames(typeof(EnemyTypes)).Length];

    private Dictionary<EnemyHolder, int> _holdersAndCountsDict = new Dictionary<EnemyHolder, int>();

    private int _totalEnemies;
    public void BeginWave()
    {
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

        

        /*_enemyCountArray[(int)EnemyTypes.JAGUAR] = _jaguars;
        _enemyCountArray[(int)EnemyTypes.FLYER] = _flyers;
        _enemyCountArray[(int)EnemyTypes.DRONE] = _drones;
        _enemyCountArray[(int)EnemyTypes.GIANT] = _giants;
        _enemyCountArray[(int)EnemyTypes.SANDMAN] = _sandmen;
        _enemyCountArray[(int)EnemyTypes.SHAMAN] = _shamans;
        _enemyCountArray[(int)EnemyTypes.SPECIAL] = _specials;

        _holdersArray[(int)EnemyTypes.JAGUAR] = jaguarHolder;
        _holdersArray[(int)EnemyTypes.FLYER] = flyerHolder;
        _holdersArray[(int)EnemyTypes.DRONE] = droneHolder;
        _holdersArray[(int)EnemyTypes.GIANT] = giantHolder;
        _holdersArray[(int)EnemyTypes.SANDMAN] = sandmanHolder;
        _holdersArray[(int)EnemyTypes.SHAMAN] = shamanHolder;
        _holdersArray[(int)EnemyTypes.SPECIAL] = specialHolder;*/
        


        _timeOnWaveStart = Time.realtimeSinceStartup;
        spawnedEnemies = new Dictionary<EnemyTypes, Stack<GameObject>>();

        /*if(_jaguars > 0)
            spawnedEnemies.Add(EnemyTypes.JAGUAR, _spawnGroups.PopulateType(jaguarHolder,_jaguars));
        if(_flyers > 0)
            spawnedEnemies.Add(EnemyTypes.FLYER, _spawnGroups.PopulateType(flyerHolder,_flyers));
        if(_drones > 0)
            spawnedEnemies.Add(EnemyTypes.DRONE, _spawnGroups.PopulateType(droneHolder,_drones));
        if(_giants > 0)
            spawnedEnemies.Add(EnemyTypes.GIANT, _spawnGroups.PopulateType(giantHolder,_giants));
        if(_sandmen > 0)
            spawnedEnemies.Add(EnemyTypes.SANDMAN, _spawnGroups.PopulateType(sandmanHolder,_sandmen));
        if(_shamans > 0)
            spawnedEnemies.Add(EnemyTypes.SHAMAN, _spawnGroups.PopulateType(shamanHolder,_shamans));
        if(_specials > 0)
            spawnedEnemies.Add(EnemyTypes.SPECIAL, _spawnGroups.PopulateType(specialHolder,_specials));*/

        Stack<GameObject> spawnedStack;
        foreach(KeyValuePair<EnemyHolder, int> HoldersCounts in _holdersAndCountsDict)
        {
            spawnedEnemies.Add(HoldersCounts.Key.enemyType, _spawnGroups.PopulateType(HoldersCounts.Key, HoldersCounts.Value));
            
            if(spawnedEnemies.TryGetValue(HoldersCounts.Key.enemyType, out spawnedStack))
                _totalEnemies += spawnedStack.Count;
        }

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

       if(unlockCondition.HasFlag(WaveUnlockConditions.KillEnemies))
       {
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
                            float percentage = 
                                (totalLiving * 100)/_totalEnemies;
                            if(percentage >= totalKillPercentage)
                            {
                                locked = false;
                                return;
                            }
                        }
                    }
                }
            }
        }

        if(unlockCondition.HasFlag(WaveUnlockConditions.KillEnemiesOfType))
        {
            int typeTotal;
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

                                    float percentage = 
                                        (typeAlive * 100) / typeTotal;
                                    if(percentage >= typeKillPercentage)
                                    {
                                        locked = false;
                                        return;
                                    }
                                }
                            }
                        }

                    }
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


    }
    


    


}
