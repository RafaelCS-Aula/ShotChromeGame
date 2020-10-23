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
using UnityEngine.UI;

namespace RoboRyanTron.Unite2017.Variables
{
    /// <summary>
    /// Sets an Image component's fill amount to represent how far Variable is
    /// between Min and Max.
    /// </summary>
    public class ImageFillSetter : MonoBehaviour
    {
        [Tooltip("Value to use as the current ")]
        public FloatVariable Variable;

        [Tooltip("Min value that Variable to have no fill on Image.")]
        public FloatVariable Min;

        [Tooltip("Max value that Variable can be to fill Image.")]
        public FloatVariable Max;

        [Tooltip("Image to set the fill amount on." )]
        public Image Image;

        private void Update()
        {
            Image.fillAmount = Mathf.Clamp01(
                Mathf.InverseLerp(Min.Value, Max.Value, Variable.Value));
        }
    }
}