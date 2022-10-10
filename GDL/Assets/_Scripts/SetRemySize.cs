using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRemySize : MonoBehaviour
{
    private Trial trial;
    private I3DVisualization manager;

    void Start()
    {
        // Get the manager of this scene - a manager always needs to inherit from the interface I3DVisualization.
        manager = GetComponentInParent<I3DVisualization>();

        trial = manager.ChosenTrial; // Fetch the trial from the manager.
        // calculate the scale depending on the measured size of Remy when scale == 1.
        float scale = trial.ParticipantSize / 3.731f; 
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
