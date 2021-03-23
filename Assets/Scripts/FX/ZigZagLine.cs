using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ZigZagLine : MonoBehaviour
{
    [SerializeField] private int lineSegments;

    [SerializeField] private float pointVariation;
    [SerializeField] private float  lineLength;
    [SerializeField] private float  duration;

     private float widthFactor;

     [SerializeField]
     private FloatVariable widthCurrentVal;
    [SerializeField] private FloatVariable witdthMaxVal;
    private Vector3 _start;
    private Vector3 _end;
    LineRenderer lineRenderer;

   // public void StorePower(FloatData power) => widthCurrentVal = power.Value;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer)
            lineRenderer.enabled = false;
    }
    public void DrawLine(Vector3 endPoint)
    {
        //print("stored power:" + widthCurrentVal);
        //print("DrawLine");
        if(lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        if(!lineRenderer) return;
        
        lineRenderer.enabled = false;
        _end = endPoint;
        _start = _end;
        _start.y += lineLength;
        float division = lineLength/lineSegments;

        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.positionCount = lineSegments;
        Vector3[] positions = new Vector3[lineSegments];
        for (int i = 0; i < positions.Length; i++)
        {
            float variation = Random.Range(-pointVariation, pointVariation);
            positions[i] = new Vector3(_start.x + variation, _start.y - division * i, _start.z + variation);

            if(i == positions.Length - 1)
                positions[i] = new Vector3(_end.x, _end.y, _end.z);
            
        }
        lineRenderer.SetPositions(positions);
        widthFactor = widthCurrentVal.Value / witdthMaxVal.Value;
        //print("line width:" + widthFactor);
        lineRenderer.widthMultiplier = widthFactor;
        lineRenderer.enabled = true;
        
        StartCoroutine(StopThunder(duration));

    }

    //TODO: Merge both methods
    public void DrawTwoPointLine(Vector3 startPoint, Vector3 endPoint)
    {
        if(lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        if(!lineRenderer) return;
        
        Debug.Log($"Drawing zigzag from {startPoint} to {endPoint}");

        lineRenderer.enabled = false;
        _end = endPoint;
        _start = startPoint;

        float length = Vector3.Distance(startPoint,endPoint);
        float division = length/lineSegments;
        Vector3 direction = (endPoint - startPoint).normalized;

        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.positionCount = lineSegments;
        Vector3[] positions = new Vector3[lineSegments];
        for (int i = 0; i < positions.Length; i++)
        {
            float variation = Random.Range(-pointVariation, pointVariation);
            positions[i] = _start + direction * (division * i) + new Vector3(variation, variation, variation);

            if(i == positions.Length - 1)
                positions[i] = new Vector3(_end.x, _end.y, _end.z);
            
        }
        lineRenderer.SetPositions(positions);
        //widthFactor = widthCurrentVal / witdthMaxVal;
        //print("line width:" + widthFactor);
        //lineRenderer.widthMultiplier = widthFactor;
        lineRenderer.enabled = true;
        Debug.Log($"Drawing zigzag from {startPoint} to {endPoint}");
        StartCoroutine(StopThunder(duration));

    }

    public void DrawMultiplePointLine(Vector3 startPoint, Vector3[] points)
    {
        
        if(lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        if(!lineRenderer) return;
        
        if(points.Length == 0)
            return;
        //Debug.Log($"Drawing zigzag from {startPoint} to {endPoint}");

        lineRenderer.enabled = false;
        _end = points[points.Length - 1];
        _start = startPoint;

        //float length = Vector3.Distance(startPoint,_end);
        //float division = length/points;
        //Vector3 direction = (endPoint - startPoint).normalized;

        lineRenderer.alignment = LineAlignment.TransformZ;
        
        Vector3 directionToNext;
        float lengthToNext;
        List<Vector3> positions = new List<Vector3>();

        positions.Add(_start);

        for(int i = 0; i < points.Length; i++)
        {
            directionToNext = (points[i] - positions[positions.Count - 1]).normalized;
            lengthToNext = Vector3.Distance(points[i], positions[i]);
            float segmentLength = (int)(lengthToNext / lineSegments);
            for(int j = 1; j <= lineSegments; j++)
            {
                float variation = Random.Range(-pointVariation, pointVariation);
                positions.Add(positions[i] + directionToNext * segmentLength * j + new Vector3(variation, variation, 0));
            }
            positions.Add(points[i]);
        }
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
        widthFactor = widthCurrentVal.Value / witdthMaxVal.Value;
        print("line width:" + widthFactor);
        lineRenderer.widthMultiplier = widthFactor;
        lineRenderer.enabled = true;
        //Debug.Log($"Drawing zigzag from {startPoint} to {endPoint}");
        StartCoroutine(StopThunder(duration));

    }

    private IEnumerator StopThunder(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lineRenderer.enabled = false;
    }

}
