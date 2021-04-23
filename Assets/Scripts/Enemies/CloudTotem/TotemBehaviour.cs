using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class TotemBehaviour : MonoBehaviour
{
    [SerializeField] private SphereCollider ThunderCover;

    [SerializeField] private FloatVariable coverHeight;

    [SerializeField] private FloatVariable coverRadius;

    [SerializeField] private FloatVariable birthToCoverDelay;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(birthToCoverDelay);
        SpawnCover();

    }

    [Button]
    public void SpawnCover()
    {
        if(ThunderCover)
        {
            Instantiate(ThunderCover.gameObject, transform.position + transform.up*coverHeight, Quaternion.identity);
            ThunderCover.isTrigger = true;
            ThunderCover.radius = coverRadius;

        }

    }
#if UNITY_EDITOR
    private void OnDrawGizmos() {

        
        Handles.color = new Color(0,0,0.1f,0.4f);
        Handles.DrawSolidDisc(transform.position + transform.up*coverHeight, transform.up,coverRadius);
        Handles.color = new Color(0,0,0.2f,0.9f);
        Handles.DrawDottedLine(transform.position, transform.position + transform.up*coverHeight, 10);
        Handles.color = Color.white;
        Handles.Label(transform.position + transform.up* (coverHeight + 2), "Thunder Cover");
    }
#endif
}
