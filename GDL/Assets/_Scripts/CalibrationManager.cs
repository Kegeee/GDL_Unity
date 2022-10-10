using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CalibrationManager : MonoBehaviour, I3DVisualization
{
    private float startDelay;
    private float targetTime;
    private float time0;
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
        animationName = trial.AnimationFile;
    }
    void Start()
    {
        startDelay = trial.SyncTime;
        targetTime = trial.TargetTime;
        time0 = Time.time;
    }

    void Update()
    {
        if((Time.time - time0 >= startDelay)&& !hasBeenLaunched)
        {
            xsensData.Play(animationName);
            hasBeenLaunched = true;
        }
        if(Time.time - time0 >= targetTime && !hasCalibrationBegun)
        {
            xsensData.speed = 0;
            GetComponentInChildren<GazeCalibration>().enabled = true;
            hasCalibrationBegun = true;
        }
    }
}