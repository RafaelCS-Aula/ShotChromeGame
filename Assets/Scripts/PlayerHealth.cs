using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // Take this out after PoC
using NaughtyAttributes;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private FloatVariable MaxPlayerHP;
    [SerializeField] private FloatData PlayerHP;

    [Scene]
    [SerializeField] private string deathScene;

    [SerializeField] private UnityEvent OnDeathEvent;

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
        SceneManager.LoadScene(deathScene);
    }
}
