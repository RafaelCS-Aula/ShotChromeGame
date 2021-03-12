using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class SpawnerHolder : MonoBehaviour
{
   [SerializeField]
    private Stack<CombatSpawner> spawners = new Stack<CombatSpawner>();
    

    [ShowNativeProperty]
    public float spawnPoints => spawners.Count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void CreateSpawner()
    {
        if(spawners.Count == 0)
            UpdateStack();
        
        GameObject go = new GameObject($"Spawner {spawners.Count}");
        CombatSpawner cs = go.AddComponent<CombatSpawner>();
        spawners.Push(cs);
        go.transform.SetParent(gameObject.transform);
        
       
    }
    
    [Button]
    public void DeleteLastSpawner()
    {
        if(spawners.Count == 0)
            UpdateStack();
        CleanStack();
        Object.DestroyImmediate(spawners.Peek().gameObject);
        spawners.Pop();
    }

    private void CleanStack()
    {
        CombatSpawner cs = spawners.Peek();

        while(cs == null)
        {
            spawners.Pop();
            cs = spawners.Peek();
        }
       
    }

    private void UpdateStack()
    {

        CombatSpawner[] spawnersInScene = 
            gameObject.GetComponentsInChildren<CombatSpawner>();
        foreach(CombatSpawner cs in spawnersInScene )
            spawners.Push(cs);

    }

}
