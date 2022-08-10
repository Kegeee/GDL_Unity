using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrajectory : MonoBehaviour
{
    public string path;
    public string fileName;
    bool saving;
   //Vector2 position = new Vector2();
    //List<Vector2> trajectory = new List<Vector2>();

    void Start()
    {
        saving = false;
        path = path  +  fileName + ".csv";
        System.IO.File.WriteAllText(path, "");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            saving = !saving;
            UnityEngine.Debug.Log(saving);
        }
        if (saving)
        {
                string line = System.String.Format("{0,3:f2};{1,3:f2}; {2,3:f2}\n", transform.position.x, transform.position.y, transform.position.z);
                System.IO.File.AppendAllText(path, line.Replace(",",".")); // append to the file
        }
    }
}
