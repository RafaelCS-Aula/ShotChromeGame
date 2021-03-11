using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName="Combat/EnemyGroup",fileName="New Enemy Group")]
public class EnemyGroup : ScriptableObject
{
    //TODO: Change this to ask for general enemy type class
    
    [SerializeField][BoxGroup("Enemy Prefabs")]
    private GameObject jaguarPrefab;

    [SerializeField][BoxGroup("Enemy Prefabs")]
    private GameObject flyerPrefab;

    [SerializeField][BoxGroup("Enemy Prefabs")]
    private GameObject dronePrefab;

    [SerializeField][BoxGroup("Enemy Prefabs")]
    private GameObject giantPrefab;

    [SerializeField][BoxGroup("Enemy Prefabs")]
    private GameObject sandManPrefab;

    [SerializeField][BoxGroup("Enemy Prefabs")]
    private GameObject shamanPrefab;
    
    private GameObject[] spawnedEnemies;

    public int JaguarAmount;
    public int FlyerAmount;

    [EnableIf("HasFlyers")]
    public int DroneAmount;
    public int GiantAmount;
    public int SandmanAmount;
    public int ShamanAmount;

    private bool HasFlyers => FlyerAmount > 0;     

    public void SpawnGroup(Transform spawnBaseTransform)
    {
        List<GameObject> enemies = new List<GameObject>();

        //Jaguars spawn on the ground
        //Flyers spawn in the air
        //Drones spawn next to flyers
        // Others spawn on the ground
        enemies.AddRange(spawnEnemyType(JaguarAmount,jaguarPrefab,spawnBaseTransform));
        enemies.AddRange(spawnEnemyType(FlyerAmount,flyerPrefab,spawnBaseTransform));

        if(HasFlyers)
            enemies.AddRange(spawnEnemyType(DroneAmount,dronePrefab,spawnBaseTransform));

        enemies.AddRange(spawnEnemyType(GiantAmount,giantPrefab,spawnBaseTransform));
        enemies.AddRange(spawnEnemyType(SandmanAmount,sandManPrefab,spawnBaseTransform));
        enemies.AddRange(spawnEnemyType(ShamanAmount,shamanPrefab,spawnBaseTransform));

    }

    private GameObject[] spawnEnemyType(int amount, GameObject prefab, Transform place)
    {
        GameObject[] e = new GameObject[amount];
        for (int i = amount; i < amount; i++)
        {
            e[i] = Instantiate(prefab,place);
        }
        return e;
    }


}
