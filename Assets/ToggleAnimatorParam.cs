using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class ToggleAnimatorParam : MonoBehaviour
{

    /*[AnimatorParam("FadePanel")]
    public string param;*/

    private void Awake() {
        //GetComponent<Animator>().StartPlayback();

    }
   private void OnEnable() {
       GetComponent<Animator>().SetBool("hello", false);
   }


}
