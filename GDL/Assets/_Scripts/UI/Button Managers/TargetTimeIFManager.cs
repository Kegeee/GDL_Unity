using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTimeIFManager : InputFieldSkeleton
{
    protected override bool TrialDone()
    {
        return selectedTrial.TargetDone;
    }
    protected override string TrialText()
    {
        return System.Convert.ToString(selectedTrial.TargetTime);
    }
    protected override void SetTrial()
    {
        selectedTrial.TargetTime = (float)System.Convert.ToDouble(thisInputField.text);
        Debug.Log($"Target time of trial {selectedTrial.TrialId} has been saved to : " +
            $"{selectedTrial.TargetTime} s.");
    }
}
