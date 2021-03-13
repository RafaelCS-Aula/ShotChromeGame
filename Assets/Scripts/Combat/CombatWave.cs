using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;



[CreateAssetMenu(menuName = "Combat/EncounterWave",fileName = "New Wave")]
public class CombatWave:ScriptableObject
{
    public UnityEvent StartWave;

    [ReadOnly][Tooltip("The encounter will not trigger the next wave while this is true")]
    public bool locked;
    [ReadOnly][Tooltip("The encounter will not be complete until all waves are marked as complete")]
    public bool complete;


    [SerializeField]
    private SpawnerHolder spawnGroups;
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

    
    
    


    


}
