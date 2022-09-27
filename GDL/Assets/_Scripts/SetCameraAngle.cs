using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraAngle : MonoBehaviour
{
    private Camera cam;
    private Trial trial;
    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    private void Start()
    {
        trial = GetComponentInParent<VisualisationManager>().ChosenTrial;
        transform.localEulerAngles = trial.CameraOffset;
    }
}
