using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CombatSpawner : MonoBehaviour
{
    [HideInInspector]
    public EnemyTypes enemy;

    private string _enemyName => enemy.ToString().ToLower();
    private string pathToIcons => $"enemies/{_enemyName}_icon.png";
    private void OnDrawGizmos() 
    {
        
       Gizmos.DrawIcon(transform.position, pathToIcons, true);
    }
}
