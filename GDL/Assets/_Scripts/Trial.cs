using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;


public class Trial
{
    // Instantiate variables.
    private float[,] storedCSV; // The data in the CSV of the trial.
    private int trialId; // The trial number.
    private string trialFile; // The file name of the csv.
    private string animationFile; // The file of the animation in fbx format.
    private Vector3 rotationOffset; // result of rotation calibration.
    private Vector3 cameraOffset; // result of camera rotation calibration
    private float syncTime; // Given time to wait for synchronisation.
    // The folowing boolean are used to make sure calibration is only done once.
    private bool rotationDone = false;
    private bool syncTimeDone = false;
    private bool cameraDone = false;

    // ===========================================================
    //  Getter and setter. 
    // ===========================================================

    // ReadOnly
    public int TrialId 
    {
        get { return trialId; } 
  
    }
    public bool RotationDone
    {
        get { return rotationDone; }
    }
    public bool SyncTimeDone
    {
        get { return syncTimeDone; }
    }
    public bool CameraDone
    {
        get { return cameraDone; }
    }
    public string TrialFile
    {
        get { return trialFile; }
    }
    public string AnimationFile
    {
        get { return animationFile; }
    }
    // You can only set the rotation offset once, at calibration.
    public Vector3 RotationOffset
    {
        set
        {
            if (rotationDone)
            {
                Debug.LogWarning("Rotation calibration has already been done.");
                return;

            }
            else
            {
                rotationOffset = value;
                rotationDone = true;
            }
        }
        get { return rotationOffset; }
    }
    // You can only set the sync time once.
    public float SyncTime
    {
        set
        {
            if (syncTimeDone)
            {
                Debug.Log("SyncTime has already been set.");
                return;
            }
            else
            {
                syncTime = value;
                syncTimeDone = true;
            }
        }
        get { return syncTime; }
    }
    // Youcan only set the camera angle once.
    public Vector3 CameraOffset
    {
        set
        {
            if (cameraDone)
            {
                Debug.Log("Camera angle has already been calibrated.");
                return;
            }
            else
            {
                cameraOffset = value;
                cameraDone = true;
            }
        }
        get { return cameraOffset; }
    }
    // ReadOnly
    public float[,] StoredCSV
    {
        get { return storedCSV; }
    }

    // ======================================================================
    // Constructeur de la classe
    public Trial (int trialNumber)
    {
        trialId = trialNumber;
        trialFile = "gazeData " + trialId + ".csv";
        animationFile = "TestFinalXsens-00" + trialId;
        storedCSV = ReadCSV(trialFile);
        rotationOffset = Vector3.zero;
        cameraOffset = Vector3.zero;
        syncTime = 0;
        rotationDone = false;
        cameraDone = false;
        syncTimeDone = false;
    }

    // ======================================================================
    // Functions.
    // ======================================================================

    // The following function reads the corresponding csv file of the trial and stores the data
    // in the storedCSV value of the object.
    private float[,] ReadCSV(string file)
    {
        // write the path of the data
        string path = $"Assets/Resources/{file}";
        // Prepare the variable to return.
        float[,] result;

        // Instanciate filereader and variable to return.
        var fileData = System.IO.File.ReadAllText(path); // File reader.
        string[] lines = fileData.Split("\n"[0]); // Array of all data in string format.
        result = new float[lines.Length - 1, 4]; // Instance of the desired data at the correct dimensions.

        // store first time stamp to offset the timeline
        string[] firstLine = lines[1].Trim().Split(","[0]);
        float firstTimeStamp = float.Parse(firstLine[0], (CultureInfo)CultureInfo.InvariantCulture);

        // instanciate column to read
        int norm_x = 1, norm_y = 2;

        // Read all data and stores it in the result variable.
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] lineData = (lines[i].Trim()).Split(","[0]);
            result[i - 1, 0] = float.Parse(lineData[norm_x].Substring(0, Math.Min(4, lineData[norm_x].Length)), (CultureInfo)CultureInfo.InvariantCulture);
            result[i - 1, 1] = float.Parse(lineData[norm_y].Substring(0, Math.Min(4, lineData[norm_y].Length)), (CultureInfo)CultureInfo.InvariantCulture);
            result[i - 1, 2] = float.Parse(lineData[0], (CultureInfo)CultureInfo.InvariantCulture);

            // To get the worldFrame in the moddified matrix data.
            result[i - 1, 3] = float.Parse(lineData[3], (CultureInfo)CultureInfo.InvariantCulture);

            // To delete the offset from the first timestamp.
            result[i - 1, 2] = result[i - 1, 2] - firstTimeStamp;

            // Used to display the  whole data in the console, for debug purposes.
            //Debug.Log(result[i - 1, 0] + "," + result[i - 1, 1] + "," + result[i - 1, 2]);
        }
        return result;
    }
}
