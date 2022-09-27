using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.Reflection;
using System.Text;
using System.IO;

public class GazeVector : MonoBehaviour
{
    // Setup variables
    private Camera cam;
    private VisualisationManager master;

    // For painting gaze on wall
    public GameObject decalPrefab;
    private Vector3 lastHitPos;

    // To get the point where we are looking, along with its desired size.
    public GameObject sphere;
    public bool paintGaze;
    public float degreeError = 2f;
    private bool instantiated = false; // used to only instantiate the sphere once

    // to get the line renderer (the component that renders our gaze vector)
    private LineRenderer lr;

    // To setup the time line with the PL.
    private float time0 = 0;
    private float[] timeVector;
    private int displayedGazeIndex = 0, count = 0; // for optimization.

    // For saving results.
    public bool saveResults = true;
    public float timeToSave = 150f;    
    public String filePath;
    public string trial = "2";
    private StringBuilder sb;
    private bool resultSaved = false;
    private int nbOfLines;
    private int[] worldFrame;
    

    private void Start()
    {
        // Getting the camera component
        cam = gameObject.GetComponent<Camera>();
        // Fetching the data loaded in the master object.
        master = gameObject.GetComponentInParent(typeof(VisualisationManager)) as VisualisationManager;
        timeVector = new float[master.ChosenTrial.StoredCSV.GetLength(0)]; // We take the number of rows in storedCSV.
        worldFrame = new int[master.ChosenTrial.StoredCSV.GetLength(0)];
        for (int j = 0; j < master.ChosenTrial.StoredCSV.GetLength(0); j++)
        {
            timeVector[j] = master.ChosenTrial.StoredCSV[j, 2];
            worldFrame[j] = Convert.ToInt16(master.ChosenTrial.StoredCSV[j,3]);
        }

        // initialise variables
        lastHitPos = new Vector3(0, 0, 0);

        // Setup saving file if needed.
        if (saveResults)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            filePath = filePath + "_" + trial + ".txt";
            sb = new StringBuilder();
            sb.AppendLine("time;hit target;world frame;");
            nbOfLines = 1;
        }

        // fetch lineRenderer component to display gaze vector
        lr = GetComponent<LineRenderer>();

        // set number of points the line will be passing through - here only two.
        lr.positionCount = 2;
        // Start and end color (doesnt work)
        // Fixed by adding a material to the lineRenderer component.
        lr.startColor = Color.green;
        lr.endColor = Color.green;

        // Setting width of the LineRenderer
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;

        // Get the time at which the gaze vector is actually started.
        /// Should always be 0, but if it isn't, it shouldn't be a problem thanks to this variable.
        time0 = Time.time;
    }
    void FixedUpdate()
    {
        int timeRow = indexOfTime(timeVector, Time.time - time0, displayedGazeIndex);
        // Direction of the ray in the 2D plan of the cam : that where you want to put eye tracking data
        Vector3 direction = new Vector3(master.ChosenTrial.StoredCSV[timeRow, 0], master.ChosenTrial.StoredCSV[timeRow, 1], 0);

        displayedGazeIndex = timeRow;

        //Debug.Log($"Timerow = {timeRow}");

        // Send Ray cast from cam head in the gaze direction
        if (!Physics.Raycast(cam.ViewportPointToRay(direction), out RaycastHit hit))
        {
            Debug.Log("Raycast failed");
            return;
        }

        // Paints on the environment the gazeHits.
        if (paintGaze)
        {
            if (count > 7) // paint only every 8 times (~30Hz) because too much disparities 
            {
                var distance = Mathf.Sqrt((lastHitPos.x - hit.point.x) * (lastHitPos.x - hit.point.x)
                    + (lastHitPos.y - hit.point.y) * (lastHitPos.y - hit.point.y)
                    + (lastHitPos.z - hit.point.z) * (lastHitPos.z - hit.point.z));
                DrawGazeOnEnvironment(hit, distance);
                count = 0;
                lastHitPos = hit.point;
            }
            else count++;
        }

        // Draw the actual gazeVector using the lineRenderer by settings the lineRenderer waypoints.
        // Start point
        lr.SetPosition(0, cam.transform.position);
        // End point 
        lr.SetPosition(1, hit.point);

        // If we want to save the timestamp when a raycast hits a target.
        if (saveResults)
        {
            if (!resultSaved)
            {
                writeResult(hit, sb, worldFrame, timeRow);
                if (Time.time >= timeToSave)
                {
                    saveResult();
                    resultSaved = true;
                    Debug.Log($"Results have been saved at \"{filePath}\" with {nbOfLines} lines.");
                }
            }
        }
        /*
         * Instantiate a sphere that will be moving to the gaze hit position.
         * This sphere'size will be dependant on the publicly defined error. (default : 2 degrees)
         */
        if (!instantiated)
        {
            sphere = Instantiate(sphere, hit.point, Quaternion.identity);
            instantiated = true;
        }
        else
        {
            sphere.transform.position = hit.point;
            sphere.transform.localScale = Vector3.one * Mathf.Sin(degreeError * Mathf.PI / 180f) *
                Vector3.Distance(gameObject.transform.position, hit.point);
        }
    }
    /*
     * This function returns the index of the next pupil gaze to display.
     * It uses the timestamps fetched from the master (ReadingCSV), and returns the index 
     * of the next time value between the time right now and the next measured time by PL.
     * Right now, it uses O.04s for PL frames, witch means 25Hz.
     * 
     * lastDisplayedRow is used for code optimization.
     */
    private int indexOfTime(float[] timeVector, float time, int lastDisplayedRow)
    {
        /*
         * for debug purposes :
        Debug.Log("Time :" + time);
        Debug.Log("lastDisplayed " + lastDisplayedRow);
        Debug.Log("timeVector[lastDisplayed] " + timeVector[lastDisplayedRow]);
        */
        int maxIndex = timeVector.Length;
        for (int j = lastDisplayedRow; j < maxIndex; j++)
        {
            if ((timeVector[j] > time) && (timeVector[j] < time + 0.04)) return j;
            // If we don't find any data after 0.04s, we notify the user with valuable info. Then we stick to the last known time.
            else if (timeVector[j] > time + 0.04)
            {
                Debug.Log($"Missing data after row n°{lastDisplayedRow} / at time : {Time.time - time0}");
                return lastDisplayedRow;
            }
            
        }
        if (timeVector[maxIndex - 1] < time) return maxIndex - 1;

        Debug.Log("Big problem, see indexOfTime in GazeVector.cs");
        return -1;
    }
    private void DrawGazeOnEnvironment(RaycastHit hit, float distance)
    {
        // Instantiate gaze decal prefab (red dot) 
        var decal = Instantiate(decalPrefab);

        // Set prefab position and orientation on hit point
        decal.transform.forward = hit.normal * -1f;
        decal.transform.position = hit.point - decal.transform.forward / 1000; // add offset so renderers of the prefab and of the environment do not merge 

        // Set red dot size according to distance between new instantied prefab and last instanciated prefab so (saccade = small dots) and (focus = big dots)
        decal.transform.localScale = new Vector3(Mathf.Max(Mathf.Min(1 / (50 * distance), 0.4f), 0.05f),
            Mathf.Max(Mathf.Min(1 / (50 * distance), 0.4f), 0.05f),    // Size of dot [0.05, 0.4], 0.05 : huge saccade, 0.4 : staring, focusing
            Mathf.Max(Mathf.Min(1 / (50 * distance), 0.4f), 0.05f));
    }    
    // The following function is used to store target hit timestamp in a stringbuilder.     
    private void writeResult(RaycastHit hit, StringBuilder sb, int[] worldFrame, int timeRow)
    {
        if(hit.transform.parent.name == "Target")
        {
            sb.AppendLine($"{Time.time};{hit.transform.name};{worldFrame[timeRow]}");
            nbOfLines++;
        }
    }
    // The following function is used to write in a txt file in csv format the data stored in a string builder.
    private void saveResult()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, sb.ToString());
        else
            File.AppendAllText(filePath, sb.ToString());
    }
}