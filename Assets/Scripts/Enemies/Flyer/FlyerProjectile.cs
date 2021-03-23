using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerProjectile : MonoBehaviour
{
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public float damage;
    [HideInInspector] public float projSpeed;
    [HideInInspector] public float destroyAfterSeconds;

    [HideInInspector] public GameEvent damageEvent;

    [SerializeField] private LayerMask layersToHit;
 
    private SphereCollider col;

    private bool hasDirection;

    private void Start()
    {
        col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        hasDirection = false;
        StartCoroutine("DestroyByTime");
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasDirection)
        {
            transform.LookAt(targetPos);
            hasDirection = true;
        }
            transform.position += transform.forward * projSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layersToHit == (layersToHit| (1 << other.gameObject.layer)))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                damageEvent.RaiseDamageArg(damage);
            }
            //print("HIT " + other.gameObject.name);
            Destroy(gameObject);
        }
    }
    private IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(destroyAfterSeconds);
        Destroy(transform.gameObject);
    }
}
