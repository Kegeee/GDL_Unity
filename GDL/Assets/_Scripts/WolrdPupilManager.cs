/*
 * This class is used to select the correct video depending on the chosen trial.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WolrdPupilManager : MonoBehaviour
{
    private int trialNumber;
    private VideoPlayer videoPlayer;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
    void Start()
    {
        trialNumber = GetComponentInParent<VisualisationManager>().ChosenTrial.TrialId;
        videoPlayer.url = @"Assets/Resources/world " + trialNumber + ".mp4";
        videoPlayer.Prepare();
    }
}
