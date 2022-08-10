using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SyncTime : MonoBehaviour
{
    public float startDelay = 7.662f;
    public int startFrameVP;
    public string animationName;
    private Animator xsensData;
    private float time0 = 0;
    private bool hasBeenLaunched = true;
    private VideoPlayer vp;
    void Start()
    {
        GameObject Remy = GameObject.Find("Remy");
        xsensData = Remy.GetComponent<Animator>();
        vp = GameObject.Find("World Pupil").GetComponent<UnityEngine.Video.VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - time0 > startDelay)&&hasBeenLaunched)
        {
            xsensData.Play(animationName);
            
            vp.Play();
            vp.frame = startFrameVP;
            hasBeenLaunched =false;
        }
    }
}
