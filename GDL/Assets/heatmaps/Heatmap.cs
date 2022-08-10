using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heatmap : MonoBehaviour
{
    public Material material;

    private List<Vector4> positions;
    private List<Vector4> properties;

    private int count;

    void Start()
    {
        count = 0;
        positions = new List<Vector4>();
        properties = new List<Vector4>();
    }

    void Update()
    {
    }

    public void AddPoint(Vector3 newPoint)
    {
        count++;
        positions.Add(new Vector4(newPoint.x, newPoint.y, newPoint.z , 0f));
        properties.Add(new Vector4(0.25f, 1f, 0f, 0f));
        material.SetInt("_Points_Length", count);
        material.SetVectorArray("_Points", positions.ToArray());
        material.SetVectorArray("_Properties", properties.ToArray());
    }
}