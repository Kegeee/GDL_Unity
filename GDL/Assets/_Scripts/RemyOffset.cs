/*
 * This script manages the avatar position and angle at the start of its animation.
 * It fetches the data angle from the selected trial and adds an animation event at the first frame (not the frame 0)
 * of the desired animation.
 * Said event launches a coroutine that : 
 *  1# offset the avatar's position to the right place.
 *  2# offset its rotation to the desired rotation.
 *  3# allows the animation to apply root motion on the next frame.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RemyOffset : MonoBehaviour
{
    private Animator animator; // The avatar's animator.
    private AnimationEvent launchOffset; // The event that launches the coroutine.
    private AnimationClip clip; // The desired animation clip.
    private Trial trial; // The trial from which data is fetched.
    private I3DVisualization manager;

    private void Awake()
    {
        // Fetch all needed data.
        animator = GetComponent<Animator>(); // Fetch animator.
        animator.applyRootMotion = false; // Make sure animator DOES NOT apply root motion.
        // It is essential so that the script's offset may not be overriden by the animator.

        launchOffset = new AnimationEvent(); // We create the event.
        launchOffset.functionName = "LaunchOffset"; // Add to the event the function it is supposed to call.
    }
    private void Start()
    {
        // The start function is used to make sure the object can communicate with the manager and all data is already fetched
        // in the manager.
        if (GetComponentInParent<VisualisationManager>() != null) manager = GetComponentInParent<VisualisationManager>();
        else if (GetComponentInParent<CalibrationManager>() != null) manager = GetComponent<CalibrationManager>();

        trial = manager.ChosenTrial; // Fetch the trial from the manager.
        // Fetch the correct animation from the animator.
        foreach ( AnimationClip animation in animator.runtimeAnimatorController.animationClips)
        {
            if (animation.name == trial.AnimationFile)
            {
                clip = animation;
            }
        }
        launchOffset.time = 1 / clip.frameRate; // Set the event to trigger at the first frame
                                                // - meaning the one after the 0 frame.
        clip.events = System.Array.Empty<AnimationEvent>(); // Just making sure it is empty even though it should be.
        clip.AddEvent(launchOffset); // Add the previously made event to the clip.       
    }
    // This function is called by the Animation Event.
    // It is only used to launch the coroutine that actually offsets the avatar.
    public void LaunchOffset()
    {
        StartCoroutine(OffsetRemy());
    }
    // This coroutine offsets the avatar.
    IEnumerator OffsetRemy()
    {
        transform.position = new Vector3(-1.5f, 0, 0); // Sets the position to (-1.5,0,0).
        transform.eulerAngles = trial.RotationOffset; // Sets the rotation to the desired rotation in the trial.
        // Calculate at which frame we currently are, so that we may display it for debug purposes.
        // The Debug should always display that the offset was done on the first frame.
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        int currentFrame = (int)(animator.GetCurrentAnimatorStateInfo(0).normalizedTime 
            * (clipInfo[0].clip.length * clipInfo[0].clip.frameRate));
        Debug.Log($"Offset called at animation \"{clip.name}\" frame n°{currentFrame}");
        yield return 0; // Wait one frame.
        animator.applyRootMotion = true; // Allow the animator to apply root motion, thus moving the avatar however he wants.
        StopCoroutine(OffsetRemy()); // Stop the coroutine.
    }
}
