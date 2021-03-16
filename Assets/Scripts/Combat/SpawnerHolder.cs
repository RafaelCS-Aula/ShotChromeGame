using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class SpawnerHolder : MonoBehaviour
{
    
    [SerializeField]
    private Stack<CombatSpawner>[] spawnerGroups = 
    {
        new Stack<CombatSpawner>(),
        new Stack<CombatSpawner>(),
        new Stack<CombatSpawner>(),
        new Stack<CombatSpawner>(),
        new Stack<CombatSpawner>(),
        new Stack<CombatSpawner>(),
        new Stack<CombatSpawner>()
    };

  
    private string[] _groupnames = 
    {
        ":: Jaguar Spawners",
        ":: Flyer Spawners",
        ":: Drone Spawners",
        ":: Giant Spawners",
        ":: Sandman Spawners",
        ":: Shaman Spawners",
        ":: Special Spawners"
    };

    [SerializeField][HideInInspector]
    private GameObject[] _groups = new GameObject[7];







#region Creation Buttons
    [Button(":::::::: Create Jaguar Spawn ::::::::")] 
    private void CreateJaguarSpawner() => 
        CreateSpawner(EnemyTypes.JAGUAR);
    [Button] private void ClearLastJaguarSpawner() => 
        DeleteLastSpawner(EnemyTypes.JAGUAR);
    
    [Button(":::::::: Create Flyer Spawn ::::::::")]
     private void CreateFlyerSpawner() => 
        CreateSpawner(EnemyTypes.FLYER);
    [Button] private void ClearLastFlyerSpawner() => 
        DeleteLastSpawner(EnemyTypes.FLYER);

    [Button(":::::::: Create Drone Spawn ::::::::")]
     private void CreateDroneSpawner() => 
        CreateSpawner(EnemyTypes.DRONE);
    [Button] private void ClearLastDroneSpawner() => 
        DeleteLastSpawner(EnemyTypes.DRONE);

    [Button(":::::::: Create Giant Spawn ::::::::")]
     private void CreateGiantSpawner() => 
        CreateSpawner(EnemyTypes.GIANT);
    [Button] private void ClearLastGiantSpawner() => 
        DeleteLastSpawner(EnemyTypes.GIANT);
    
    [Button(":::::::: Create Sandman Spawn ::::::::")]
     private void CreateSandmanSpawner() => 
        CreateSpawner(EnemyTypes.SANDMAN);
    [Button] private void ClearLastSandmanSpawner() => 
        DeleteLastSpawner(EnemyTypes.SANDMAN);
    
    [Button(":::::::: Create Shaman Spawn ::::::::")]
     private void CreateShamanSpawner() => 
        CreateSpawner(EnemyTypes.SHAMAN);
    [Button] private void ClearLastShamanSpawner() => 
        DeleteLastSpawner(EnemyTypes.SHAMAN);
    
    [Button(":::::::: Create Special Spawn ::::::::")]
     private void CreateSpecialSpawner() => 
        CreateSpawner(EnemyTypes.SPECIAL);
    [Button] private void ClearLastSpecialSpawner() => 
        DeleteLastSpawner(EnemyTypes.SPECIAL);
#endregion
    
    private void Awake() 
    {
        
        foreach(CombatSpawner cs in GetComponentsInChildren<CombatSpawner>())
        {   
            
            spawnerGroups[(int)cs.enemy].Push(cs);
        }

    }

    private void CreateSpawner(EnemyTypes enemyType)
    {
        
        Stack<CombatSpawner> choosenStack = spawnerGroups[(int)enemyType];
        GameObject parent = _groups[(int)enemyType]; 
        

        if(parent == null)
        {
            parent = new GameObject(_groupnames[(int)enemyType]);
            _groups[(int)enemyType] = parent; 
            parent.transform.position = gameObject.transform.position;
            parent.transform.SetParent(gameObject.transform);
           
            
        }


        
        SyncStackToScene(choosenStack, parent);
        
        GameObject go = new GameObject($"{enemyType.ToString()} Spawner {parent.transform.childCount} - {gameObject.name}");
        CombatSpawner cs = go.AddComponent<CombatSpawner>();
        cs.enemy = enemyType;
        
        
        go.transform.SetParent(parent.transform);
        if(choosenStack.Count > 1)
        {
            print($"Position of {choosenStack.Peek().name}: {choosenStack.Peek().transform.position}");
            print($"Local Position of {choosenStack.Peek().name}: {choosenStack.Peek().transform.localPosition}");

            go.transform.localPosition = choosenStack.Peek().transform.localPosition;
        }
            
        else
        {
            go.transform.localPosition = parent.transform.localPosition;
        }
        

        choosenStack.Push(cs);
        parent.name = _groupnames[(int)enemyType] + $"- {parent.transform.childCount}";
       

       
        /*for(int i = 0; i < spawnerGroups.Length; i++)
        {
            Debug.Log($"Stack of {(CombatSpawnerTypes)i} - has {spawnerGroups[i].Count}");
        }
        Debug.Log("======================================");*/
    }
    
    
    private void DeleteLastSpawner(EnemyTypes enemyType)
    {
        Stack<CombatSpawner> choosenStack = spawnerGroups[(int)enemyType];
        if(choosenStack.Count == 0)
            SyncStackToScene(choosenStack, 
                choosenStack.Peek().transform.parent.gameObject);
        CleanStack(choosenStack);
        Object.DestroyImmediate(choosenStack.Peek().gameObject);
        GameObject parent = _groups[(int)enemyType];
        parent.name = _groupnames[(int)enemyType] + $"- {parent.transform.childCount}";
        choosenStack.Pop();
    }

    private void CleanStack(Stack<CombatSpawner> stack)
    {
        if(stack.Count == 0)
            return;
        CombatSpawner cs = stack.Peek();

        while(cs == null)
        {
            stack.Pop();
            cs = stack.Peek();
        }
       
    }

    private void SyncStackToScene(Stack<CombatSpawner> stack, GameObject group)
    {
        CleanStack(stack);
        CombatSpawner[] spawnersInScene = 
            group.GetComponentsInChildren<CombatSpawner>();
        foreach(CombatSpawner cs in spawnersInScene )
            stack.Push(cs);

    }


    public Stack<GameObject> PopulateType(EnemyHolder enemy, int amount, CombatWave callerWave)
    {
        EnemyTypes enemyType = enemy.enemyType;
        Stack<GameObject> spawned = new Stack<GameObject>();

        CombatSpawner[] sp = spawnerGroups[(int)enemyType].ToArray();
        if(sp.Length == 0)
        {
            Debug.Log($"No spawners of type {enemyType}");
            return null; 
        }
            
        int enqueued = 0;

        while(enqueued < amount)
        {
            for(int i = 0; i < sp.Length; i++)
            {
                
                sp[i].spawnQueue.Enqueue(enemy.enemyPrefab);
                enqueued++;
                if(enqueued >= amount)
                    break;
            }

        }
        foreach(CombatSpawner cs in sp)
        {
            spawned.Push(cs.StartSpawning(callerWave,enemy.distanceToAllowOtherToSpawn));
        }
        return spawned;

    }

   

}
