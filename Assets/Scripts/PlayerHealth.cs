using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private FloatVariable MaxPlayerHP;
    [SerializeField] private FloatData PlayerHP;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHP.SetValue(MaxPlayerHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHP <= 0) Die();
    }

    public void GetDamaged(float damageValue)
    {
        PlayerHP.SetValue(PlayerHP - damageValue);
    }

    private void Die()
    {
        if (transform.parent == null) Destroy(gameObject);

        else Destroy(transform.root.gameObject);
    }
}
