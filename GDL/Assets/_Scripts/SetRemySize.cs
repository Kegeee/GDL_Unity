using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRemySize : MonoBehaviour
{
    private Trial trial;
    private I3DVisualization manager;

    void Start()
    {
        if (GetComponentInParent<VisualisationManager>() != null) manager = GetComponentInParent<VisualisationManager>();
        else if(GetComponentInParent<CalibrationManager>() != null) manager = GetComponent<CalibrationManager>();

        trial = manager.ChosenTrial; // Fetch the trial from the manager.
        // calculate the scale depending on the measured size of Remy when scale == 1.
        float scale = trial.ParticipantSize / 3.731f; 
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
