using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSpawner : MonoBehaviour
{
    public CombatSpawnerTypes enemy;


    private void OnDrawGizmosSelected() 
    {
        switch(enemy)
        {
            case(CombatSpawnerTypes.JAGUAR):
            Gizmos.color = Color.black;
            break;
            case(CombatSpawnerTypes.FLYER):
            Gizmos.color = Color.blue;
            break;
            case(CombatSpawnerTypes.DRONE):
            Gizmos.color = Color.cyan;
            break;
            case(CombatSpawnerTypes.GIANT):
            Gizmos.color = Color.white;
            break;
            case(CombatSpawnerTypes.SANDMAN):
            Gizmos.color = Color.green;
            break;
            case(CombatSpawnerTypes.SHAMAN):
            Gizmos.color = Color.red;
            break;
            case(CombatSpawnerTypes.SPECIAL):
            Gizmos.color = Color.yellow;
            break;
        }
       
        if(enemy == CombatSpawnerTypes.SPECIAL)
            Gizmos.DrawCube(transform.position,Vector3.one * 0.5f);
        else
            Gizmos.DrawSphere(transform.position,0.5f);
    }
}
