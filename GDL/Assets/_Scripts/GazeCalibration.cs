using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class GazeCalibration : MonoBehaviour
{
    // Set up variables
    public Camera cam;
    private LineRenderer lr;
    private bool isDone = false;
    private Transform targetTransform;
    private float previousError;
    private TextMeshProUGUI displayedText;

    // Set up direction variables for the calibration.
    private bool left = true;
    private bool right = true;
    private bool up = true;
    private bool down = true;

    // undistorted pixels from undistorted_dot.py. Has to be set up everytime for calibration.
    public float[] gazeToTarget = new float[2] { 1148.5620981692657f, 236.42494212490305f };

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
        Debug.Log(displayedText.text);
    }
    void FixedUpdate()
    {
        // Direction of the ray in the 2D plan of the cam : that where you want to put eye tracking data.
        // On Y axis : 1 - gaze to account for differences in convention between openCV and openGL.
        Vector3 direction = new Vector3(gazeToTarget[0], 1080 - gazeToTarget[1], 0); 

        // Send Ray cast from cam head in the gaze direction.
        // If the raycast fails, display it in the console and then stop the function.
        if (!Physics.Raycast(cam.ScreenPointToRay(direction), out RaycastHit hit))
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
            // Si l'erreur précédente est plus grande que l'erreur actuelle, on continue de tourner vers la droite.
            if (previousError >= Vector3.Distance(hit.point, targetTransform.position))
            {
                // On store la nouvelle erreur.
                previousError = Vector3.Distance(hit.point, targetTransform.position);
                // Et on tourne la caméra.
                Vector3 rotation = new Vector3(0, 0.0001f, 0);
                cam.transform.Rotate(rotation);
            }
            // Sinon on tourne dans la mauvaise direction.Il faut donc corriger la dernière fois qu'on a tourné à droite.
            else
            {
                Vector3 rotation = new Vector3(0, -0.0001f, 0);
                cam.transform.Rotate(rotation);
                right = false;
            }
        }
        // Ensuite on regarde si il ne faut pas tourner la caméra à gauche.
        else if (left)
        {
            if (previousError >= Vector3.Distance(hit.point, targetTransform.position))
            {
                // On store la nouvelle valeure
                previousError = Vector3.Distance(hit.point, targetTransform.position);
                // on tourne la caméra
                Vector3 rotation = new Vector3(0, -0.0001f, 0);
                cam.transform.Rotate(rotation);
            }
            else
            {
                Vector3 rotation = new Vector3(0, 0.0001f, 0);
                cam.transform.Rotate(rotation);
                left = false;
            }
        }
        // On passe à haut/bas.
        else if (up)
        {
            // Si l'erreur précédente est plus grande que l'erreur actuelle, on continue de tourner vers la le haut.
            if (previousError >= Vector3.Distance(hit.point, targetTransform.position))
            {
                // On store la nouvelle erreur.
                previousError = Vector3.Distance(hit.point, targetTransform.position);
                // Et on tourne la caméra.
                Vector3 rotation = new Vector3(0.0001f, 0, 0);
                cam.transform.Rotate(rotation);
            }
            // Sinon on tourne dans la mauvaise direction.Il faut donc corriger la dernière fois qu'on a tourné en haut.
            else
            {
                Vector3 rotation = new Vector3(-0.0001f, 0, 0);
                cam.transform.Rotate(rotation);
                up = false;
            }
        }
        // On essaie vers le bas.
        else if (down)
        {
            if (previousError >= Vector3.Distance(hit.point, targetTransform.position))
            {
                // On store la nouvelle valeure
                previousError = Vector3.Distance(hit.point, targetTransform.position);
                // on tourne la caméra
                Vector3 rotation = new Vector3(-0.0001f ,0, 0);
                cam.transform.Rotate(rotation);
            }
            else
            {
                Vector3 rotation = new Vector3(0.0001f, 0, 0);
                cam.transform.Rotate(rotation);
                down = false;
            }
        }

        // Start point
        lr.SetPosition(0, cam.transform.position);
        // End point 
        lr.SetPosition(1, hit.point);

        displayedText.SetText(gameObject.transform.rotation.eulerAngles.ToString());
    }
}