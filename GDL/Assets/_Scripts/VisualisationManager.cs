/*
 * This manager manages the visualisation. It is mostly a mashup of "SyncTime" and 
 * "ReadingCSV" in order for the rest of the visualisation to work smoothly using the Trial class.
 * In the future, the handling of events such as pausing the visualisation will be done here.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System;
using System.Globalization;

public class VisualisationManager : MonoBehaviour
{
    // Variables
    private Trial trial;
    public Trial ChosenTrial
    {
        get { return trial; }
    }

    // To sync time; launch animation, etc...
    private float timeO = 0;
    private float startDelay;
    private string animationName;
    private GameObject Remy;
    private Animator xsensData;
    private bool hasBeenLaunched = false;
    private bool videoPlaying = false;
    private VideoPlayer vp;
    private TextMeshProUGUI videoFrame;
    void Awake()
    {
        // Instantiate all needed data from the Trial given by the CanvasManager.
        trial = CanvasManager.instance.GetTrial;
        startDelay = trial.SyncTime;
        animationName = trial.AnimationFile;

        Remy = GameObject.Find("Remy");
        xsensData = Remy.GetComponent<Animator>();
        vp = GameObject.Find("World Pupil").GetComponent<VideoPlayer>();
        vp.Prepare();

        videoFrame = GameObject.Find("VideoFrame").GetComponent<TextMeshProUGUI>();

        GetComponentInChildren<GazeVector>().enabled = true;

        timeO = Time.time;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (vp.isPrepared)
        {
            if (!videoPlaying)
            {
                vp.Play();
                videoPlaying = true;
            }
            videoFrame.SetText($"frame : {vp.frame}");
        }
        if ((Time.time - timeO > startDelay) && !hasBeenLaunched && vp.isPrepared)
        {
            vp.time = Time.time - timeO;
            xsensData.Play(animationName);
            hasBeenLaunched = true;
        }
        //Debug.Log("Vidéo player time : " + vp.time + "/ Unity time : " + (Time.time - timeO));
    }
}
