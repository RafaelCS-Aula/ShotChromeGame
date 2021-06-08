using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.Events;

[RequireComponent(typeof(Spawner))]
public class TotemBehaviour : MonoBehaviour
{

    public UnityEvent<Vector3> OnCoverHit;

    [SerializeField] private SphereCollider ThunderCover;

    [SerializeField] private FloatVariable coverHeight;

    [SerializeField] private FloatVariable coverRadius;

    [SerializeField] private FloatVariable birthToCoverDelay;

    [SerializeField] private float ammoSpawnRange;

    [SerializeField] private LayerMask geometrylayer;

    private GameObject _spawnedCover;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(birthToCoverDelay);
        SpawnCover();

    }

    [Button]
    public void SpawnCover()
    {
       
        
        if(ThunderCover)
        {
            _spawnedCover = Instantiate(ThunderCover.gameObject, transform.position + transform.up * coverHeight, Quaternion.identity);
             Transform spawnTrn = _spawnedCover.transform;
            if(GetComponent<ParticleAttractor>())
                GetComponent<ParticleAttractor>().attractionPoint = spawnTrn;

            _spawnedCover.GetComponent<SphereCollider>().isTrigger = true;
            _spawnedCover.GetComponent<SphereCollider>().radius = coverRadius;

            ThunderReactor _tReact = 
                _spawnedCover.GetComponent<ThunderReactor>();
            if(_tReact)
            {
                _tReact.OnHitByThunder += SpawnAmmo;
                print("Found Reactor in Cover");
            }

        }

    }

    private void SpawnAmmo()
    {
        print("Spawning Ammo");

        bool spotFound = false;
        int wallHitCount = 0;

        Vector3 spawnPoint = transform.position;
        
        while(!spotFound)
        {
            spawnPoint.x += Random.Range(-ammoSpawnRange, ammoSpawnRange);
            spawnPoint.z += Random.Range(-ammoSpawnRange, ammoSpawnRange);
            spawnPoint.y += 1;

            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, (transform.position - spawnPoint).normalized,out hitInfo, ammoSpawnRange + 5, geometrylayer))
            {
                wallHitCount++;
                if(wallHitCount > 10)
                {
                    spawnPoint = hitInfo.point - (spawnPoint - transform.position).normalized;
                    spotFound = true;
                }
                else
                {
                    wallHitCount = 0;
                    spotFound = true;
                }
                

            }
            else
            {
                spotFound = true;
            }

        }
        OnCoverHit.Invoke(spawnPoint);
    }

    private void OnDestroy() {
        Destroy(_spawnedCover);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos() {

        
        Handles.color = new Color(0,0,0.1f,0.4f);
        Handles.DrawSolidDisc(transform.position + transform.up*coverHeight, transform.up,coverRadius);
        Handles.color = new Color(0,0,0.2f,0.9f);
        Handles.DrawDottedLine(transform.position, transform.position + transform.up*coverHeight, 10);
        Handles.color = Color.white;
        Handles.Label(transform.position + transform.up* (coverHeight + 2), "Thunder Cover");

        Handles.DrawWireDisc(transform.position + transform.up, transform.up,ammoSpawnRange);
        
    }
#endif
}
