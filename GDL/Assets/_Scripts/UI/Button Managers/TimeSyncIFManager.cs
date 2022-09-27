using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSyncIFManager : MonoBehaviour
{
    private CanvasManager canvasManager;
    private TMP_InputField thisInputField;
    private Trial selectedTrial;
    private bool displayIF = true;
    private TMP_Text thisText; 
    // Start is called before the first frame update
    void Awake()
    {
        canvasManager = GetComponentInParent<CanvasManager>();
        thisInputField = GetComponent<TMP_InputField>();
        thisText = GetComponentInChildren<TMP_Text>();
        canvasManager.OnTrialSelected += OnTrialUpdate;

        if (!displayIF)
        {
            Debug.Log("honk");
            thisInputField.readOnly = true;
            thisInputField.text = "";
            thisText.SetText(System.Convert.ToString(selectedTrial.SyncTime));
            thisText.color = Color.green;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // When the sync time has not already been given to the trial and we press enter while the input field is on focus, we feed the trial the float value
        // in the input field.
        if(displayIF && thisInputField.isFocused && Input.GetKeyUp(KeyCode.Return))
        {
            try
            {
                selectedTrial.SyncTime = (float)System.Convert.ToDouble(thisInputField.text);
                Debug.Log($"Time synchronization of trial {selectedTrial.TrialId} has been saved to : {selectedTrial.SyncTime}s");
                displayIF = false;
            }
            catch
            {
                Debug.LogWarning("Could no convert your input to float. Time synchronization not saved.");
            }
        }
        else if (!displayIF)
        {
            thisInputField.readOnly = true;
            thisInputField.text = "";
            thisText.SetText(System.Convert.ToString(selectedTrial.SyncTime));
            thisText.color = Color.green;
        }
    }
    // If the sync time is done on this trial, we should grey out the input field.
    private void OnTrialUpdate(Trial trial)
    {
        selectedTrial = trial;
        displayIF = !selectedTrial.SyncTimeDone;
        if (displayIF)
        {
            thisInputField.readOnly = false;
            thisText.SetText("");
            thisText.color = Color.black;
            thisInputField.text = "";
        }
        else
        {
            thisInputField.readOnly = true;
            thisText.SetText(System.Convert.ToString(selectedTrial.SyncTime));
            thisText.color = Color.green;
        }
    }
}
