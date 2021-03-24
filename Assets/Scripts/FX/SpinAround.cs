using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{

    [SerializeField]
    private Vector3Variable speed;

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(speed);
    }
}
