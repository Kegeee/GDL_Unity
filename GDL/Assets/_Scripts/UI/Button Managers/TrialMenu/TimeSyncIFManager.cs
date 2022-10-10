/*
 * Inherits from input field skeleton.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSyncIFManager : InputFieldSkeleton
{
    protected override bool TrialDone()
    {
        return selectedTrial.SyncTimeDone;
    }
    protected override string TrialText()
    {
        return System.Convert.ToString(selectedTrial.SyncTime);
    }
    protected override void SetTrial()
    {
        selectedTrial.SyncTime = (float)System.Convert.ToDouble(thisInputField.text);
        Debug.Log($"Synchronization time of trial {selectedTrial.TrialId} has been saved to : " +
            $"{selectedTrial.SyncTime} s.");
    }
}
