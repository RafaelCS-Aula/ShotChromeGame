using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float Health = 100;
    private void Update()
    {
        if (Health <= 0) Destroy(gameObject);
    }

    public void OnDamaged(float damage)
    {
        Health -= damage;
        print(Health);
    }
}
