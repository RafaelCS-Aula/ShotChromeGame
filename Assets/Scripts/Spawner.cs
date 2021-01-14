using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    public void Spawn() => Instantiate(objectToSpawn, transform.position, transform.rotation);
    public void SpawnAt(Vector3 spawnPoint) => Instantiate(objectToSpawn, spawnPoint, transform.rotation);
}
