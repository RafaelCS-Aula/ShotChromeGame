using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] FloatVariable MaxHealth;
    private float health = 100;

    private void Start()
    {
        health = MaxHealth;
    }
    private void Update()
    {
        if (health <= 0) Destroy(gameObject);
    }

    public void OnDamaged(float damage)
    {
        health -= damage;
        print(health);
    }
}
