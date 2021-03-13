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


    private GameObject[] _groups = new GameObject[7];







#region Creation Buttons
    [Button(":::::::: Create Jaguar Spawn ::::::::")] 
    public void CreateJaguarSpawner() => 
        CreateSpawner(EnemyTypes.JAGUAR);
    [Button] public void ClearLastJaguarSpawner() => 
        DeleteLastSpawner(EnemyTypes.JAGUAR);
    
    [Button(":::::::: Create Flyer Spawn ::::::::")]
     public void CreateFlyerSpawner() => 
        CreateSpawner(EnemyTypes.FLYER);
    [Button] public void ClearLastFlyerSpawner() => 
        DeleteLastSpawner(EnemyTypes.FLYER);

    [Button(":::::::: Create Drone Spawn ::::::::")]
     public void CreateDroneSpawner() => 
        CreateSpawner(EnemyTypes.DRONE);
    [Button] public void ClearLastDroneSpawner() => 
        DeleteLastSpawner(EnemyTypes.DRONE);

    [Button(":::::::: Create Giant Spawn ::::::::")]
     public void CreateGiantSpawner() => 
        CreateSpawner(EnemyTypes.GIANT);
    [Button] public void ClearLastGiantSpawner() => 
        DeleteLastSpawner(EnemyTypes.GIANT);
    
    [Button(":::::::: Create Sandman Spawn ::::::::")]
     public void CreateSandmanSpawner() => 
        CreateSpawner(EnemyTypes.SANDMAN);
    [Button] public void ClearLastSandmanSpawner() => 
        DeleteLastSpawner(EnemyTypes.SANDMAN);
    
    [Button(":::::::: Create Shaman Spawn ::::::::")]
     public void CreateShamanSpawner() => 
        CreateSpawner(EnemyTypes.SHAMAN);
    [Button] public void ClearLastShamanSpawner() => 
        DeleteLastSpawner(EnemyTypes.SHAMAN);
    
    [Button(":::::::: Create Special Spawn ::::::::")]
     public void CreateSpecialSpawner() => 
        CreateSpawner(EnemyTypes.SPECIAL);
    [Button] public void ClearLastSpecialSpawner() => 
        DeleteLastSpawner(EnemyTypes.SPECIAL);
#endregion
    
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

}
