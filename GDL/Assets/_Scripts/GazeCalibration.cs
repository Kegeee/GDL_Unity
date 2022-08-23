using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class GazeCalibration : MonoBehaviour
{
    // Set up variables
    public Camera cam; // To get the cam component.
    public float precision = 0.01f; // To set up the precision of the calibration.
    public bool angleCalibration = true;
    private LineRenderer lr; // For debug purposes, LineRenderer is used to display the current RayCast.
    private bool isDone = false; // To setup the cartesian error between the raycast hit and the target.
    private float previousError;
    private Transform targetTransform; // To get the target position.
    private TextMeshProUGUI displayedText; // To display the camera angles in world degree.

    // Set up direction variables for the calibration.
    private bool left = true;
    private bool right = true;
    private bool up = true;
    private bool down = true;

    // undistorted pixels from undistorted_dot.py. Has to be set up everytime for calibration.
    public float[] gazeToTarget = new float[2] { 0.616012648748197f, 0.64134309875092f};

    // calibration result :
    private static Vector3 rotationResult = Vector3.zero;

    private void Start()
    {
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

        // To display cam transform.
        displayedText = GameObject.Find("CamData").GetComponent<TextMeshProUGUI>();
    }
    void FixedUpdate()
    {
        // Direction of the ray in the 2D plan of the cam : that where you want to put eye tracking data.
        // On Y axis : 1 - gaze to account for differences in convention between openCV and openGL.
        Vector3 direction = new Vector3(gazeToTarget[0], gazeToTarget[1], 0); 

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
            Debug.Log("Direction : right" + " / erreur : " + previousError);
            if (angleCalibration)
            {
                turnCamera(ref right, hit.point, targetTransform.position, ref previousError, 0, precision, angleCalibration);
            }
            else
            {
                turnCamera(ref right, hit.point, targetTransform.position, ref previousError, precision, 0, angleCalibration);
            }
        }
        // Ensuite on regarde si il ne faut pas tourner la caméra à gauche.
        else if (left)
        {
            Debug.Log("Direction : left" + " / erreur : " + previousError);
            if (angleCalibration)
            {
                turnCamera(ref left, hit.point, targetTransform.position, ref previousError, 0, -precision, angleCalibration);
            }
            else
            {
                turnCamera(ref left, hit.point, targetTransform.position, ref previousError, -precision, 0, angleCalibration);
            }
        }
        // On passe à haut/bas.
        else if (up)
        {
            Debug.Log("Direction : up" + " / erreur : " + previousError);
            if (angleCalibration)
            {
                turnCamera(ref up, hit.point, targetTransform.position, ref previousError, -precision, 0, angleCalibration);
            }
            else
            {
                turnCamera(ref up, hit.point, targetTransform.position, ref previousError, 0, -precision, angleCalibration);
            }
        }
        // On essaie vers le bas.
        else if (down)
        {
            Debug.Log("Direction : down" + " / erreur : " + previousError);
            if (angleCalibration)
            {
                turnCamera(ref down, hit.point, targetTransform.position, ref previousError, precision, 0, angleCalibration);
            }
            else
            {
                turnCamera(ref down, hit.point, targetTransform.position, ref previousError, 0, precision, angleCalibration);
            }
        }

        // Start point
        lr.SetPosition(0, cam.transform.position);
        // End point 
        lr.SetPosition(1, hit.point);

        // Display 
        displayedText.SetText(rotationResult.ToString());
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
    private void turnCamera(ref bool direction, Vector3 point, Vector3 target,ref float previousError, float xRot, float yRot, bool angleCalibration)
    {
        // Si l'erreur précédente est plus grande que l'erreur actuelle, on continue de tourner dans la même direction.
        if (previousError >= Vector3.Distance(point, target))
        {
            // On store la nouvelle valeure
            previousError = Vector3.Distance(point, target);

            // On tourne la caméra.
            Vector3 rotation = new Vector3( xRot, yRot, 0);
            if(angleCalibration) cam.transform.Rotate(rotation);
            else cam.transform.Translate(rotation);
            // On incrémente rotationResult
            rotationResult += rotation;
        }
        // Sinon on tourne dans la mauvaise direction. Il faut donc corriger la dernière fois qu'on a tourné dans cette direction.
        else
        {
            //Vector3 rotation = new Vector3(-xRot, -yRot, 0);
            Vector3 rotation = new Vector3( -xRot, -yRot, 0);
            if(angleCalibration) cam.transform.Rotate(rotation);
            else cam.transform.Translate(rotation);
            direction = false;
            // On incrémente rotationResult
            rotationResult += rotation;
        }
    }
}