using UnityEngine;
using System.Collections;
using System;
using TMPro;
using System.Linq;

public class GazeCalibration : MonoBehaviour
{
    // Manager.
    CalibrationManager manager;

    // Set up variables
    private Camera cam; // To get the cam component.
    private float precision = 0.01f; // To set up the precision of the calibration.
    private LineRenderer lr; // For debug purposes, LineRenderer is used to display the current RayCast.
    private bool isDone = false; // To setup the cartesian error between the raycast hit and the target.
    private float previousError;
    private Transform targetTransform; // To get the target position.

    // Set up direction boolean for the calibration.
    private bool left = true;
    private bool right = true;
    private bool up = true;
    private bool down = true;

    // Undistorted pixels from undistorted_dot.py. Taken from the manager, this should 
    // undistorted pixels from undistorted_dot.py. Has to be set up everytime for calibration.
    private double[] gazeToTarget;

    // calibration result :
    private static Vector3 rotationResult = Vector3.zero;

    // Raising event when the calibration is finished.
    public event OnCalibrationFinishedDelegate OnCalibrationFinished;
    public delegate void OnCalibrationFinishedDelegate(Vector3 calibrationResult);
    private bool eventRaised = false; // To raise the event only one frame.

    private void Awake()
    {
        // Get the manager.
        manager = GetComponentInParent<CalibrationManager>();

        // Get camera component.
        cam = GetComponent<Camera>();

        // set up the line renderer width and color.
        lr = GetComponent<LineRenderer>();

        lr.positionCount = 2;
        // Start and end color (doesnt work)
        // Fixed by adding a material to the lineRenderer component.
        lr.startColor = Color.green;
        lr.endColor = Color.green;

        // Setting width of the LineRenderer
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;

        // Get the target transform.
        targetTransform = GameObject.Find("Target").GetComponent<Transform>();
    }
    private void Start()
    {
        gazeToTarget = GetGazeValue();
    }
    private void OnEnable()
    {
        rotationResult = Vector3.zero;
    }
    void FixedUpdate()
    {
        // Direction of the ray in the 2D plan of the cam : that where you want to put eye tracking data.
        // On Y axis : 1 - gaze to account for differences in convention between openCV and openGL.
        Vector3 direction = new Vector3((float)gazeToTarget[0], (float)gazeToTarget[1], 0); 

        // Send Ray cast from cam head in the gaze direction.
        // If the raycast fails, display it in the console and then stop the function.
        if (!Physics.Raycast(cam.ViewportPointToRay(direction), out RaycastHit hit))
        {
            Debug.Log("Raycast failed");
            return;
        }

        // If not already done, set up initial error.
        // previous error : distance between raycast hit and target position at the last loop.
        if (!isDone)
        {
            previousError = Vector3.Distance(hit.point, targetTransform.position);
            isDone = true;
        }

        if (right)
        {
            //Debug.Log("Direction : right" + " / erreur : " + previousError);
            turnCamera(ref right, hit.point, targetTransform.position, ref previousError, 0, precision);
        }
        // Ensuite on regarde si il ne faut pas tourner la caméra à gauche.
        else if (left)
        {
            //Debug.Log("Direction : left" + " / erreur : " + previousError);
            turnCamera(ref left, hit.point, targetTransform.position, ref previousError, 0, -precision);
        }
        // On passe à haut/bas.
        else if (up)
        {
            //Debug.Log("Direction : up" + " / erreur : " + previousError);
            turnCamera(ref up, hit.point, targetTransform.position, ref previousError, -precision, 0);
        }
        // On essaie vers le bas.
        else if (down)
        {
            //Debug.Log("Direction : down" + " / erreur : " + previousError);
            turnCamera(ref down, hit.point, targetTransform.position, ref previousError, precision, 0);
        }

        // Start point
        lr.SetPosition(0, cam.transform.position);
        // End point 
        lr.SetPosition(1, hit.point);

        // If everything is finished, we raise the the finished caliration event.
        if (!eventRaised && !(up || down || left || right))
        {
            if (OnCalibrationFinished != null) OnCalibrationFinished(rotationResult);
            else { Debug.LogWarning("No listeners added yet !"); }
            eventRaised = true;
        }
    }
    /*
     * The function turnCamera takes as argument :
     * direction : the reference to the boolean direction you want to turn the camera to.
     * point : the current hit position of the raycast from the camera.
     * target : the target position.
     * previous error : the reference to the previous error from the last frame raycast.
     * xRot : how much you want the camera to turn on the x axis, in world degrees.
     * yRot : how much you want the camera to turn on the y axis, in world degrees.
     * 
     * It checks wether or not turning the cam on the desired direction was successful. If it wasn't then it 
     * corrects the camera angle. Then it changes the boolean variable direction to false. 
     * If it was successful, then it turns the camera more in the same direction, with precision° angle, on the 
     * specified axis. It also stores the current error in previousError.
     * 
     * Finally, turnCamera increments rotationResult.
     */
    private void turnCamera(ref bool direction, Vector3 point, Vector3 target,ref float previousError, float xRot, float yRot)
    {
        // Si l'erreur précédente est plus grande que l'erreur actuelle, on continue de tourner dans la même direction.
        if (previousError >= Vector3.Distance(point, target))
        {
            // On store la nouvelle valeure
            previousError = Vector3.Distance(point, target);

            // On tourne la caméra.
            Vector3 rotation = new Vector3( xRot, yRot, 0);
            cam.transform.Rotate(rotation);
            // On incrémente rotationResult
            rotationResult += rotation;
        }
        // Sinon on tourne dans la mauvaise direction. Il faut donc corriger la dernière fois qu'on a tourné dans cette direction.
        else
        {
            Vector3 rotation = new Vector3( -xRot, -yRot, 0);
            cam.transform.Rotate(rotation);
            direction = false;
            // On incrémente rotationResult
            rotationResult += rotation;
        }
    }
    // Get the desired gaze value depending on target time.
    // Not sure if that's the best way to do it as I always set this value manually.
    // Very poorly optimized because we are forced to loop through all the data since there seem to be some timestamp artifacts.
    private double[] GetGazeValue()
    {
        // Fetch gaze data.
        float[,] gazeData = manager.ChosenTrial.StoredCSV;

        // Initialise variables.
        double[] result = new double[2];

        double[] timeTarget = new double[gazeData.GetLength(0)];

        // Create a vector which stores the difference between the target time and all timestamps possible.
        for (int i = 0; i< gazeData.GetLength(0); i++)
        {
            timeTarget[i] = Math.Abs(gazeData[i, 2] - manager.ChosenTrial.TargetTime);
        }

        // Get the index of the min value - meaning the index of the timestamp closest to the target time.
        int index = Array.IndexOf(timeTarget, timeTarget.Min());
        // Feed the result the desired data and return it.
        result[0] = gazeData[index, 0]; result[1] = gazeData[index, 1];
        Debug.Log($"final index : {index} / result : {result[0]} , {result[1]} / time : {gazeData[index, 2]}");
        return result;
    }
}