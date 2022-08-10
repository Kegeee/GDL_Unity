using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTime : MonoBehaviour
{
    public float startDelay = 7.662f;
    public string animationName;
    private Animator xsensData;
    private float time0 = 0;
    private bool hasBeenLaunched = true;
  
    void Start()
    {
        GameObject Remy = transform.GetChild(1).gameObject;
        xsensData = Remy.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - time0 > startDelay)&&hasBeenLaunched)
        {
            xsensData.Play(animationName);
            hasBeenLaunched=false;
        }
    }
}
