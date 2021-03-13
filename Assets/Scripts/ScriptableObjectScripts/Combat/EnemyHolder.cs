using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName="Combat/EnemyHolder",fileName="New Enemy")]
public class EnemyHolder : ScriptableObject
{
    [SerializeField]
    private GameObject enemyPrefab;

    public EnemyTypes enemyType;
    
    


}
