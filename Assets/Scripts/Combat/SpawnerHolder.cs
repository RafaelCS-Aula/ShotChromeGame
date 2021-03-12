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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    
    [Button] public void CreateJaguarSpawner() => 
        CreateSpawner(CombatSpawnerTypes.JAGUAR);
    [Button] public void ClearJaguarSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.JAGUAR);
    
    [Button] public void CreateFlyerSpawner() => 
        CreateSpawner(CombatSpawnerTypes.FLYER);
    [Button] public void ClearFlyerSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.FLYER);

    [Button] public void CreateDroneSpawner() => 
        CreateSpawner(CombatSpawnerTypes.DRONE);
    [Button] public void ClearDroneSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.DRONE);

    [Button] public void CreateGiantSpawner() => 
        CreateSpawner(CombatSpawnerTypes.GIANT);
    [Button] public void ClearGiantSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.GIANT);
    
    [Button] public void CreateSandmanSpawner() => 
        CreateSpawner(CombatSpawnerTypes.SANDMAN);
    [Button] public void ClearSandmanSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.SANDMAN);
    
    [Button] public void CreateShamanSpawner() => 
        CreateSpawner(CombatSpawnerTypes.SHAMAN);
    [Button] public void ClearShamanSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.SHAMAN);
    
    [Button] public void CreateSpecialSpawner() => 
        CreateSpawner(CombatSpawnerTypes.SPECIAL);
    [Button] public void ClearSpecialSpawner() => 
        DeleteLastSpawner(CombatSpawnerTypes.SPECIAL);

    
    private void CreateSpawner(CombatSpawnerTypes enemyType)
    {
        
        Stack<CombatSpawner> choosenStack = spawnerGroups[(int)enemyType];
        GameObject parent = _groups[(int)enemyType]; 
        if(parent == null)
        {
            parent = new GameObject(_groupnames[(int)enemyType]);
            _groups[(int)enemyType] = parent; 
            parent.transform.SetParent(gameObject.transform);
            
        }


        if(choosenStack.Count == 0)
            UpdateStack(choosenStack);
        
        GameObject go = new GameObject($"Spawner {choosenStack.Count}");
        CombatSpawner cs = go.AddComponent<CombatSpawner>();
        choosenStack.Push(cs);
        go.transform.SetParent(parent.transform);
        
       
    }
    
    
    private void DeleteLastSpawner(CombatSpawnerTypes enemyType)
    {
        Stack<CombatSpawner> choosenStack = spawnerGroups[(int)enemyType];
        if(choosenStack.Count == 0)
            UpdateStack(choosenStack);
        CleanStack(choosenStack);
        Object.DestroyImmediate(choosenStack.Peek().gameObject);
        choosenStack.Pop();
    }

    private void CleanStack(Stack<CombatSpawner> stack)
    {
        CombatSpawner cs = stack.Peek();

        while(cs == null)
        {
            stack.Pop();
            cs = stack.Peek();
        }
       
    }

    private void UpdateStack(Stack<CombatSpawner> stack)
    {

        CombatSpawner[] spawnersInScene = 
            gameObject.GetComponentsInChildren<CombatSpawner>();
        foreach(CombatSpawner cs in spawnersInScene )
            stack.Push(cs);

    }

}
