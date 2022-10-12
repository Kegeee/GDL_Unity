using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseAnimationFinishedEvent : MonoBehaviour
{
    private Animator animator;
    private AnimationEvent animationFinished;
    private Trial trial;
    private AnimationClip clip;

    public event OnAnimationFinishedDelegate OnAnimationFinished;
    public delegate void OnAnimationFinishedDelegate();

    void Awake()
    {
        animator = GetComponent<Animator>();
        animationFinished = new AnimationEvent();
        animationFinished.functionName = "raiseEvent";

        OnAnimationFinished += honk;
    }
    private void Start()
    {
        trial = CanvasManager.instance.GetTrial;

        // Fetch the correct animation from the animator.
        foreach (AnimationClip animation in animator.runtimeAnimatorController.animationClips)
        {
            if (animation.name == trial.AnimationFile)
            {
                clip = animation;
            }
        }

        animationFinished.time = clip.length - 1 / clip.frameRate;

        clip.AddEvent(animationFinished);
    }
    private void raiseEvent()
    {
        if (OnAnimationFinished != null) OnAnimationFinished();
        else Debug.LogWarning("No listeners added yet to event OnAnimationFinished !");
    }
    private void honk()
    {
        Debug.Log("honk");
    }
}
