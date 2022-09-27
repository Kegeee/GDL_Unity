using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TempWalkScript : MonoBehaviour
{
    string filePath = @"E:\Code\MATLAB\Present-day\Pipeline\Pipeline20211108\trial 8.csv";
    string line;
    string[] parsedLine;
    StreamReader reader;
    Vector3 oldPosition;
    Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        reader = new StreamReader(filePath);
        oldPosition = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            parsedLine = line.Split(',');
            transform.position = new Vector3(float.Parse(parsedLine[0]), float.Parse(parsedLine[2]), float.Parse(parsedLine[1]));
            moveDirection = transform.position - oldPosition;
            transform.forward = moveDirection.normalized;
            oldPosition = transform.position;
        }
    }
}
