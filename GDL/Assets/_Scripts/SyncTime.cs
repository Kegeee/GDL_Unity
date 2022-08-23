using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class SyncTime : MonoBehaviour
{
    public float startDelay = 7.662f;
    public string animationName;
    private Animator xsensData;
    private bool hasBeenLaunched = false;
    private bool videoPlaying = false;
    private VideoPlayer vp;
    private TextMeshProUGUI videoFrame;
    //public int startFrame = 73;
    private bool test = false;
    void Start()
    {
        GameObject Remy = GameObject.Find("Remy");
        xsensData = Remy.GetComponent<Animator>();
        vp = GameObject.Find("World Pupil").GetComponent<UnityEngine.Video.VideoPlayer>();
        vp.Prepare();

        videoFrame = GameObject.Find("VideoFrame").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (test)
        {
            Debug.Log($"frame = {vp.frame}");
            test = false;
        }
        if (vp.isPrepared)
        {
            if (!videoPlaying)
            {
                vp.Play();
                videoPlaying = true;
            }
            videoFrame.SetText($"frame : {vp.frame}"); //$"desired frame = {startFrame} / frame = {vp.frame}");
        }
        if ((Time.time > startDelay) && !hasBeenLaunched && vp.isPrepared)
        {
            vp.time = Time.time;
            xsensData.Play(animationName);
            hasBeenLaunched = true;
            test = true;
        }   

        //Debug.Log("Vidéo player time : " + vp.time + "/ Unity time : " + Time.time);
    }
}