using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ValidateCameraButton : MonoBehaviour
{
    private CanvasController controller;
    private CanvasManager canvasManager;
    private Button thisButton;
    private TMP_InputField[] inputFields;
    private Trial selectedTrial;

    void Awake()
    {
        // Get already instantiated controllers and managers.
        controller = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();
        canvasManager.OnTrialSelected += OnTrialUpdate; // Subscribe to the trial selected event.

        thisButton = GetComponent<Button>();

        inputFields = transform.parent.GetComponentsInChildren<TMP_InputField>();

        thisButton.onClick.AddListener(OnValidateButton);
    }
    private void OnTrialUpdate(Trial newTrial)
    {
        selectedTrial = newTrial;
    }
    private void OnValidateButton()
    {
        try
        {
            float x = (float)Convert.ToDouble(inputFields[0].text);
            float y = (float)Convert.ToDouble(inputFields[1].text);
            float z = (float)Convert.ToDouble(inputFields[2].text);

            selectedTrial.CameraOffset = new Vector3(x, y, z);

            Debug.Log($"Camera offset of trial {selectedTrial.TrialId} has been " +
                $"saved to : {selectedTrial.CameraOffset}.");
        }
        catch
        {
            Debug.LogWarning("Could not convert value to Camera Offset.");
        }
    }
}
