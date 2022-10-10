using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class CalibrationOKButtonManager : MonoBehaviour
{
    private CanvasController controller;
    private Button thisButton;
    private Button reloadButton;
    private Image thisBackground;
    private Vector3 calibrationResult;

    void Awake()
    {
        controller = GetComponentInParent<CanvasController>();
        thisButton = GetComponent<Button>();

        thisButton.onClick.AddListener(OnButtonClicked);

        // Get the reload button in order to listen to it.
        reloadButton = transform.parent.GetComponentsInChildren<Button>().Where(button => button.name == "ReloadCalibrationButton").First();
        reloadButton.onClick.AddListener(OnReloadClicked);

        // Get the button background to turn it off later.
        thisBackground = GetComponent<Image>();

        // Subscribe to the scene loaded event.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Inform the controller we are ready.
        controller.allUIs[transform.GetSiblingIndex()] = true;
    }
    private void Start()
    {
        // Make sure the button stays hidden while calibration is not complete.
        hideButton(true);
    }
    // When the button is clicked, we save the results in the trial and switch to the menu scene.
    private void OnButtonClicked()
    {
        Vector3 cameraOffset = new Vector3(-16.313f, 0, -2.683f) + calibrationResult;
        GetComponentInParent<CanvasManager>().GetTrial.CameraOffset = cameraOffset;
        Debug.Log($"Camera offset saved to : {cameraOffset}.");
        SceneManager.LoadScene("StartingScene");
        CanvasManager.instance.SwitchCanvas(CanvasType.TrialMenu);
    }
    // This shows the button when the calibration finished event is raised.
    private void OnCalibrationFinished(Vector3 result)
    {
        hideButton(false);
        calibrationResult = result;
    }
    // This handler is used to subscribe to the calibration finished event as soon as possible - meaning right after the 
    // calibration scene has finished loading.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "initialiseAngleGDL") return;
        else
        {
            GameObject.Find("Head cam").GetComponent<GazeCalibration>().OnCalibrationFinished += OnCalibrationFinished;
        }
    }
    // If set to true, this function hides the button. If set to false, it shows the button.
    private void hideButton(bool hide)
    {
        thisBackground.enabled = !hide;
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(!hide);
    }
    // This handler hides the button if it sees the reload button clicked.
    private void OnReloadClicked()
    {
        hideButton(true);
    }
}
