using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class GenerateIntersections : MonoBehaviour
{
    public String data="intersections.txt";
    public Transform intersectionsHolder;
    public GameObject Prefab;
    public bool Visualize;

    private StreamWriter outputFile;

    private void Start()
    {
        if(!File.Exists(data))
            File.Create(data);
        outputFile = new StreamWriter(data);
        outputFile.AutoFlush = true;
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            outputFile.WriteLine(hit.point.x.ToString("F3")+";"+ hit.point.z.ToString("F3") + ";"+hit.point.y.ToString("F3"));
            if (Visualize)
            {
                //New visualization implement post processing
                //Heatmap heatmapHolder = hit.collider.gameObject.GetComponent<Heatmap>();
                //if(heatmapHolder!=null)
                //    heatmapHolder.AddPoint(hit.point);
                // Old visualization uncomment for use
                GameObject newIntersection = Instantiate(Prefab, hit.point, Quaternion.identity);
                newIntersection.transform.parent = intersectionsHolder;
            }
        }
    }

    private void OnApplicationQuit()
    {
        outputFile.Flush();
        outputFile.Close();
    }
}
