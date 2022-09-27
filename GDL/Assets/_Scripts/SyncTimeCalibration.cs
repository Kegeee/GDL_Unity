using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SyncTimeCalibration : MonoBehaviour
{
    public float startDelay = 7.662f;
    public float targetTime = 10.294f;
    public string animationName;
    private Animator xsensData;
    private bool hasBeenLaunched = false;
    void Start()
    {
        GameObject Remy = GameObject.Find("Remy");
        xsensData = Remy.GetComponent<Animator>();
    }

    void Update()
    {
        if((Time.time >= startDelay)&& !hasBeenLaunched)
        {
            xsensData.Play(animationName);
            hasBeenLaunched = true;
        }
        if(Time.time >= targetTime)
        {
            xsensData.speed = 0;
            GameObject.Find("Head cam").GetComponent<GazeCalibration>().enabled = true;
        }
    }
}