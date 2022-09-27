/* ===========================================================================
 * 
 * 
 * 
 * 
 * 
 *          THIS IS OUTDATED AND SHOULD NOT BE USED ANYMORE !
 *     Not deleting it just to make sure the new code works properly.
 * 
 * 
 * 
 * 
 * ============================================================================
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEditor;
using System;
using System.Globalization;

public class ReadingCSV : MonoBehaviour
{
    public float[,] storedCSV; // has public for ease of use, may setup a get/set later
    public int trial = 2; // to select trial
    public string undistorted = "original"; // to select if we want the undistorted data or the raw undistort no undistortion

    // Start is called before the first frame update
    void Start()
    {
        // write the path of the data
        string path;
        string fileName;
        if (undistorted == "undistorted")
        {
            fileName = "undistorted_gaze_";
        }
        else if (undistorted == "modified")
        {
            fileName = "gazeData ";
        }
        else
        {
            fileName = "gaze_positions_";
        }
        path = $"Assets/Resources/{fileName}{trial}.csv";
        var fileData = System.IO.File.ReadAllText(path);
        string[] lines = fileData.Split("\n"[0]);
        storedCSV = new float[lines.Length - 1, 3];

        // store first time stamp to offset the timeline
        string[] firstLine = lines[1].Trim().Split(","[0]);
        float firstTimeStamp = float.Parse(firstLine[0], (CultureInfo)CultureInfo.InvariantCulture);

        // changes column order if using undistorted data or not

        int norm_x, norm_y;

        if (undistorted == "modified" || undistorted == "undistorted")
        {
            norm_x = 1;
            norm_y = 2;
            if(undistorted == "modified") storedCSV = new float[lines.Length - 1, 4];
        }
        else
        {
            norm_x = 3;
            norm_y = 4;
        }

        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] lineData = (lines[i].Trim()).Split(","[0]);
            storedCSV[i - 1, 0] = float.Parse(lineData[norm_x].Substring(0, Math.Min(4, lineData[norm_x].Length)), (CultureInfo)CultureInfo.InvariantCulture);
            storedCSV[i - 1, 1] = float.Parse(lineData[norm_y].Substring(0, Math.Min(4, lineData[norm_y].Length)), (CultureInfo)CultureInfo.InvariantCulture);
            storedCSV[i - 1, 2] = float.Parse(lineData[0], (CultureInfo)CultureInfo.InvariantCulture);

            // To get the worldFrame in the moddified matrix data.
            if (undistorted == "modified") storedCSV[i - 1, 3] = float.Parse(lineData[3], (CultureInfo)CultureInfo.InvariantCulture);

            // To delete the offset from the first timestamp.
            storedCSV[i - 1, 2] = storedCSV[i-1,2] - firstTimeStamp;

            // Used to display the  whole data in the console.
            //Debug.Log(storedCSV[i - 1, 0] + "," + storedCSV[i - 1, 1] + "," + storedCSV[i - 1, 2]);
        }
        // Starts GazeVector Script
        GameObject.Find("Head cam").GetComponent<GazeVector>().enabled = true;
    }
}
