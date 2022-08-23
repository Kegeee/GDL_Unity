using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.Reflection;

public class GazeVector : MonoBehaviour
{
    // Setup variables
    private Camera cam;

    // For painting gaze on wall
    public GameObject decalPrefab;
    private Vector3 lastHitPos;
    // To get the point where we are looking, along with its desired size.
    public GameObject sphere;
    public float sphereSize;
    public bool paintGaze;
    private bool instantiated = false; // used to only instantiate the sphere once

    // to get the line renderer (the component that renders our gaze vector)
    private LineRenderer lr;

    // To setup the time line with the PL.
    private float time0 = 0;
    private float[] timeVector;
    private int displayedGazeIndex = 0, count = 0; // for optimization.

    // For the screenshots
    public float screenTime = 46.803f;



    ReadingCSV master;

    private void Start()
    {
        // Getting the camera component
        cam = gameObject.GetComponent<Camera>();
        // Fetching the data loaded in the master object.
        master = gameObject.GetComponentInParent(typeof(ReadingCSV)) as ReadingCSV;
        timeVector = new float[master.storedCSV.GetLength(0)]; // We take the number of rows in storedCSV.
        for (int j = 0; j < master.storedCSV.GetLength(0); j++)
        {
            timeVector[j] = master.storedCSV[j, 2];
        }
        lastHitPos = new Vector3(0, 0, 0);
        lr = GetComponent<LineRenderer>();

        lr.positionCount = 2;
        // Start and end color (doesnt work)
        // Fixed by adding a material to the lineRenderer component.
        lr.startColor = Color.green;
        lr.endColor = Color.green;

        // Setting width of the LineRenderer
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;

        time0 = Time.time;
    }
    void FixedUpdate()
    {
        int timeRow = indexOfTime(timeVector, Time.time - time0, displayedGazeIndex);
        // Direction of the ray in the 2D plan of the cam : that where you want to put eye tracking data
        Vector3 direction = new Vector3(master.storedCSV[timeRow, 0], master.storedCSV[timeRow, 1], 0);

        displayedGazeIndex = timeRow;

        // Send Ray cast from cam head in the gaze direction
        if (!Physics.Raycast(cam.ViewportPointToRay(direction), out RaycastHit hit))
        {
            //Debug.Log("Raycast failed");
            return;
        }
        if (paintGaze)
        {
            if (count > 7) // paint only every 8 times (~30Hz) because two much disparities 
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
        //Debug.Log("Variable timeRow : " + timeRow + " X : " + master.storedCSV[timeRow, 0] + " / Y : " + master.storedCSV[timeRow, 1]);
        //Debug.Log(hit.point);
        // Start point
        lr.SetPosition(0, cam.transform.position);
        // End point 
        lr.SetPosition(1, hit.point);

        if (!instantiated)
        {
            sphere = Instantiate(sphere, hit.point, Quaternion.identity);
            instantiated = true;
        }
        else
        {
            sphere.transform.position = hit.point;
            sphere.transform.localScale = Vector3.one * Mathf.Sin(2 * Mathf.PI / 180f) * Vector3.Distance(gameObject.transform.position, hit.point);
        }

    }
    /*
     * This function returns the index of the next pupil gaze to display.
     * It uses the timestamps fetched from the master (ReadingCSV), and returns the index 
     * of the next time value between the time right now and the next measured time by PL.
     * Right now, it uses O.04s for PL frames, witch means 25Hz.
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
        }
        //if (timeVector[maxIndex - 1] < time) return maxIndex - 1;

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
}