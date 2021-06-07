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

    [SerializeField] private Vector3 ammoSpawnLocation;

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
            _spawnedCover = Instantiate(ThunderCover.gameObject, transform.position + transform.up*coverHeight, Quaternion.identity);
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
        OnCoverHit.Invoke(transform.position + ammoSpawnLocation);
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

        Gizmos.DrawSphere(transform.position + ammoSpawnLocation, 0.2f);
        Handles.Label(transform.position + ammoSpawnLocation,"Spits ammo from here");
    }
#endif
}
