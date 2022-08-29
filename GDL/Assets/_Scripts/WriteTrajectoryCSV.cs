/*
 * The sole purpose of this script is to write in a txt file (that should be read as CSV) the trajectory of the walking animation.
 * May never be of use again but who knows ? 
 * Just attach it to any GameObject, the script will save in a txt file its trajectory.
 */

using System;
using System.IO;
using System.Text;
using UnityEngine;

public class WriteTrajectoryCSV : MonoBehaviour
{
    public string filePath;
    public char delimiter = ';';
    private StringBuilder sb;
    bool isDone = false;
    void Start()
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
        filePath = filePath +"_" +DateTime.Now.ToString("yyyy_mm_dd_hh_mm_ss") + ".txt";
        sb = new StringBuilder();
        sb.AppendLine("X;Y;Z;Time;");
    }

    void Update()
    {
        Vector3 position = transform.position;

        float[] output = new float[]{
         position.x,
         position.y,
         position.z,
         Time.time
        };


        int length = output.Length;

        var ligne = "";

        for (int index = 0; index < length; index++)
            ligne += Convert.ToString(output[index]) + delimiter;
        sb.AppendLine(ligne);

        while(!isDone && Time.time > 150f) 
        {
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, sb.ToString());
            else
                File.AppendAllText(filePath, sb.ToString());
            isDone = true;
        }
    }
}