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


    private AudioSource aS;
    [SerializeField] AudioClip[] hitSounds;

    [SerializeField] private bool playRandom;
    [SerializeField] private int defaultClip;

    void Start()
    {
        aS = GetComponent<AudioSource>();
        PlayerHP.SetValue(MaxPlayerHP.Value);
    }

    void Update()
    {
        if (PlayerHP <= 0) Die();
        if(PlayerHP > MaxPlayerHP)
            PlayerHP.SetValue(MaxPlayerHP.Value);
    }

    public void GetDamaged(float damageValue)
    {
        PlayerHP.SetValue(PlayerHP - damageValue);
    }

    private void Die()
    {
        SceneManager.LoadScene(deathScene);
    }

    public void HitNoise()
    {
        if (playRandom)
        {
            int clip = Random.Range(0, hitSounds.Length);
            aS.PlayOneShot(hitSounds[clip]);
        }
        else aS.PlayOneShot(hitSounds[defaultClip]);
    }
}
