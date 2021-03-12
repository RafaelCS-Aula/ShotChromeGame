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
        CreateSpawner(CombatSpawnerTypes.JAGUAR);
    [Button] public void ClearLastJaguarSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.JAGUAR);
    
    [Button(":::::::: Create Flyer Spawn ::::::::")]
     public void CreateFlyerSpawner() => 
        CreateSpawner(CombatSpawnerTypes.FLYER);
    [Button] public void ClearLastFlyerSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.FLYER);

    [Button(":::::::: Create Drone Spawn ::::::::")]
     public void CreateDroneSpawner() => 
        CreateSpawner(CombatSpawnerTypes.DRONE);
    [Button] public void ClearLastDroneSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.DRONE);

    [Button(":::::::: Create Giant Spawn ::::::::")]
     public void CreateGiantSpawner() => 
        CreateSpawner(CombatSpawnerTypes.GIANT);
    [Button] public void ClearLastGiantSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.GIANT);
    
    [Button(":::::::: Create Sandman Spawn ::::::::")]
     public void CreateSandmanSpawner() => 
        CreateSpawner(CombatSpawnerTypes.SANDMAN);
    [Button] public void ClearLastSandmanSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.SANDMAN);
    
    [Button(":::::::: Create Shaman Spawn ::::::::")]
     public void CreateShamanSpawner() => 
        CreateSpawner(CombatSpawnerTypes.SHAMAN);
    [Button] public void ClearLastShamanSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.SHAMAN);
    
    [Button(":::::::: Create Special Spawn ::::::::")]
     public void CreateSpecialSpawner() => 
        CreateSpawner(CombatSpawnerTypes.SPECIAL);
    [Button] public void ClearLastSpecialSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.SPECIAL);
#endregion
    
    private void CreateSpawner(CombatSpawnerTypes enemyType)
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
    
    
    private void DeleteLastSpawner(CombatSpawnerTypes enemyType)
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
