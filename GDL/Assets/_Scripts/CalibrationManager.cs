using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CalibrationManager : MonoBehaviour, I3DVisualization
{
    private float startDelay;
    private float targetTime;
    private string animationName;
    private Animator xsensData;
    private bool hasBeenLaunched = false;
    private bool hasCalibrationBegun = false;

    private Trial trial;
    public Trial ChosenTrial
    {
        get { return trial; }
    }
    void Awake()
    {
        xsensData = GetComponentInChildren<Animator>();
        trial = CanvasManager.instance.GetTrial;
    }
    void Start()
    {
        startDelay = trial.SyncTime;
        targetTime = trial.TargetTime;
    }

    void Update()
    {
        if((Time.time >= startDelay)&& !hasBeenLaunched)
        {
            xsensData.Play(animationName);
            hasBeenLaunched = true;
        }
        if(Time.time >= targetTime && !hasCalibrationBegun)
        {
            xsensData.speed = 0;
            GameObject.Find("Head cam").GetComponent<GazeCalibration>().enabled = true;
            hasCalibrationBegun = true;
        }
    }
}