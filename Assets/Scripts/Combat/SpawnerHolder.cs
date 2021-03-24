using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class SpawnerHolder : MonoBehaviour
{
    // Stacks of spawners, each index is a different type of spawner
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

    // Names for the parent game objects that will hold the spawners
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

    [SerializeField][ReadOnly]
    private List<CombatSpawner> SpawnersDebugList;





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
        SpawnersDebugList = new List<CombatSpawner>();
        // Go trough the hierarqui to make sure the stacks are synced to what 
        // the user sees
        foreach(CombatSpawner cs in GetComponentsInChildren<CombatSpawner>())
        {   
            SpawnersDebugList.Add(cs);
            spawnerGroups[(int)cs.enemy].Push(cs);
        }

    }

    /// <summary>
    /// Create an empty game object with a spawner component
    /// </summary>
    /// <param name="enemyType"> The enemy that will spawn from the new 
    /// spawner</param>
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

        // Put new spawner in the location & rotation of the last made spawner 
        // of the same type if it exists, if it doesn't put it in the group 
        // holder object's location
        if(choosenStack.Count > 1)
        {
            //print($"Position of {choosenStack.Peek().name}: {choosenStack.Peek().transform.position}");
            //print($"Local Position of {choosenStack.Peek().name}: {choosenStack.Peek().transform.localPosition}");

            go.transform.localPosition = choosenStack.Peek().transform.localPosition;
            go.transform.localRotation = choosenStack.Peek().transform.localRotation;
        }
            
        else
        {
            go.transform.localPosition = parent.transform.localPosition;
            go.transform.localRotation = parent.transform.localRotation;
        }
        

        choosenStack.Push(cs);

        // Update the parent's name to show how many children (spawners) it
        // has.
        parent.name = _groupnames[(int)enemyType] + $"- {parent.transform.childCount}";
       
    }
    
    /// <summary>
    /// Delete the object of the last placed spawner a type.
    /// </summary>
    /// <param name="enemyType">The type of spawner to be considered</param>
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

    /// <summary>
    /// Remove null spawners from the tip of the stack
    /// </summary>
    /// <param name="stack">The stack to clean</param>
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

    /// <summary>
    /// Syncs the stacks with the spawner components in the hierarchy.
    /// </summary>
    /// <param name="stack">The stack to sync</param>
    /// <param name="group">The parent game object of that spawner type.</param>
    private void SyncStackToScene(Stack<CombatSpawner> stack, GameObject group)
    {
        CleanStack(stack);
        CombatSpawner[] spawnersInScene = 
            group.GetComponentsInChildren<CombatSpawner>();
        foreach(CombatSpawner cs in spawnersInScene )
            stack.Push(cs);

    }

    /// <summary>
    /// Tell the spawners of a type to queue in the prefabs to instantiate, and 
    /// then spawn them.
    /// </summary>
    /// <param name="enemy">The enemy Holder with the prefab to spawn</param>
    /// <param name="amount">How many enemies?</param>
    /// <param name="delay">Wait some seconds until telling each spawner to 
    /// start</param>
    /// <param name="callerWave"> The combat wave that is sending the enemies
    /// </param>
    /// <returns>The objects spawned by all the spawners of this type.</returns>
    public Stack<GameObject> PopulateType(EnemyHolder enemy, int amount, float delay, CombatWave callerWave)
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

        // Tell each spawner how many enemies it needs to spawn
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

        // Start the spawning process, making each spawner go down their queue.
        foreach(CombatSpawner cs in sp)
        {
           /* float startTime = Time.unscaledTime;
            float time_delta = 0;
            do
            {
                time_delta = Time.unscaledTime - startTime;
                
            }while(time_delta < delay);*/

            spawned.Push(cs.StartSpawning(callerWave,enemy.distanceToAllowOtherToSpawn));
        }
        return spawned;

    }

   

}
