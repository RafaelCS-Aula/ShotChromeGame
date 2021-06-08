using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderVisuals : MonoBehaviour
{
    [SerializeField] private GameObject planePrefab;
    [SerializeField] private GameObject view;


    [SerializeField] private float lifeTime;
    [SerializeField] private float expandTime;

    [SerializeField] private FloatData thunderPower;

    [SerializeField] private float maxWidth;


    private float _counter;
    public void ThunderStrike(Vector3 hitLocation)
    {
        GameObject inst = Instantiate(planePrefab,hitLocation,Quaternion.identity);
        inst.transform.LookAt(view.transform,transform.up);
        inst.transform.rotation *= Quaternion.Euler(0, 180, 0);
        Vector3 scale = inst.transform.localScale;
        Vector3 newScale = new Vector3(0, scale.y, scale.z);
        inst.transform.localScale = newScale;
        _counter = 0;
        StartCoroutine(UpdateThunder(inst));

    }

    private IEnumerator UpdateThunder(GameObject obj)
    {
        Vector3 originalScale = obj.transform.localScale;
        Vector3 finalScale = new Vector3(maxWidth, originalScale.y, originalScale.z);
        do
        {
            
            obj.transform.localScale = Vector3.Lerp(originalScale, finalScale, _counter/expandTime);

            _counter += Time.deltaTime;
            obj.transform.LookAt(view.transform,transform.up);
            obj.transform.rotation *= Quaternion.Euler(0, 180, 0);
            
            
            yield return null;
        }while(_counter <= lifeTime);

        Destroy(obj);
        yield return null;
        
        


    }

}
