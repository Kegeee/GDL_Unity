/*
 * Abstract class used to reduce code repetition across the solution for each input fields that set the trial object.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InputFieldSkeleton : MonoBehaviour
{
    protected CanvasManager canvasManager;
    protected TMP_InputField thisInputField;
    protected Trial selectedTrial;
    protected bool displayIF = true;
    protected TMP_Text thisText;
    // Get in awake all needed objects - managers, controllers, etc... and subscribe to needed events.
    virtual protected void Awake()
    {
        canvasManager = GetComponentInParent<CanvasManager>();
        thisInputField = GetComponent<TMP_InputField>();
        thisText = GetComponentInChildren<TMP_Text>();
        canvasManager.OnTrialSelected += OnTrialUpdate;

        thisInputField.text = "0";
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        // When the participant trial has not already been set, if we press
        // enter while the input field is on focus, we feed the trial the float value
        // in the input field.
        if (displayIF && thisInputField.isFocused && Input.GetKeyUp(KeyCode.Return))
        {
            try
            {
                SetTrial();
                displayIF = false;
            }
            catch
            {
                Debug.LogWarning("Could no convert your input to float. Trial has not been saved.");
            }
        }
        else if (!displayIF)
        {
            thisInputField.readOnly = true;
            thisInputField.text = "";
            thisText.SetText(TrialText());
            thisText.color = Color.green;
        }
    }
    // If the participant size is done on this trial, we should grey out the input field.
    protected void OnTrialUpdate(Trial trial)
    {
        selectedTrial = trial;
        displayIF = !TrialDone();
        if (displayIF)
        {
            thisInputField.readOnly = false;
            thisText.SetText("");
            thisText.color = Color.black;
            thisInputField.text = "0";
        }
        else
        {
            thisInputField.readOnly = true;
            thisText.SetText(TrialText());
            thisText.color = Color.green;
        }
    }
    protected abstract bool TrialDone();
    protected abstract string TrialText();
    protected abstract void SetTrial();
}
