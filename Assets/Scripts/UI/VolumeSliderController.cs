using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSliderController : MonoBehaviour
{

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private FloatData _currentVolume;

    public void ChangeVolume(float targetVolume)
    {
        _mixer.SetFloat("MasterVolume",Mathf.Log10(targetVolume) * 20);
        _currentVolume?.SetValue(Mathf.Log10(targetVolume) * 20);
    }
}
