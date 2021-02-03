// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

/// -----------------
/// Altered by Rafael Castro e Silva.
/// 
/// 22/10/2020
/// ----------------

using UnityEngine;

namespace RoboRyanTron.Unite2017.Variables
{
    public class VariableAudioTrigger : MonoBehaviour
    {
        public AudioSource AudioSource;

        public FloatVariable Variable;

        public FloatVariable LowThreshold;

        private void Update()
        {
            if (Variable.Value < LowThreshold.Value)
            {
                if (!AudioSource.isPlaying)
                    AudioSource.Play();
            }
            else
            {
                if (AudioSource.isPlaying)
                    AudioSource.Stop();
            }
        }
    }
}