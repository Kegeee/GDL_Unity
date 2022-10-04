using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchCalibrationButtonManager : MonoBehaviour
{
    private Trial trial;
    private CanvasController canvasController;
    private CanvasManager canvasManager;
    private Button thisButton;
    private bool eventHandled = false;
    private bool trialDone = false;
    void Awake()
    {
        // Get already instantiated controllers and managers.
        canvasManager = GetComponentInParent<CanvasManager>();
        canvasController = GetComponentInParent<CanvasController>();

        thisButton = GetComponent<Button>();

        thisButton.onClick.AddListener(OnButtonClicked);
        thisButton.interactable = false;
    }
    private void Start()
    {
        // Subscribe to the trial selection event.
        canvasManager.OnTrialSelected += OnTrialUpdate;
        // Inform the controller that this UI has finished loading.
        canvasController.allUIs[transform.GetSiblingIndex()] = true;
    }
    void Update()
    {
        // The following condition takes into account that on the first frame the trial variable may not be populated.
        // In such a case, we have to use the default value of trialDone, which should always render the button unclickable.
        // We can only get inside this if statement in one frame thanks to the eventHandled variable.
        if (trial != null ? trial.TargetDone : trialDone && !eventHandled)
        {
            thisButton.interactable = true;
            eventHandled = true;
        }
    }
    // If the target time has already been set we may launch calibration right away. 
    // Else we have to force the user into setting the target time himself.
    private void OnTrialUpdate(Trial newTrial)
    {
        // new trial is the trial we are working with, make sure to store it ! Bad Youri >:C !!!
        trial = newTrial;
        trialDone = trial.TargetDone;
        if (trialDone)
        {
            thisButton.interactable = true;
            eventHandled = true;
        }
        else
        {
            thisButton.interactable = false;
            eventHandled = false;
        }
    }
    private void OnButtonClicked()
    {
        SceneManager.LoadScene("initialiseAngleGDL");
        canvasManager.turnOffPopUp();
        canvasManager.SwitchCanvas(CanvasType.CameraCalibration);
    }
}
