using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SizeIfManager : InputFieldSkeleton
{
    protected override bool TrialDone()
    {
        return selectedTrial.SizeDone;
    }
    protected override string TrialText()
    {
        return System.Convert.ToString(selectedTrial.ParticipantSize);
    }
    protected override void SetTrial()
    {
        selectedTrial.ParticipantSize = (float)System.Convert.ToDouble(thisInputField.text);
        Debug.Log($"Participant size of trial {selectedTrial.TrialId} has been saved to : " +
            $"{selectedTrial.ParticipantSize} m.");
    }
}
