using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalibrateButtonManager : MonoBehaviour
{
    private CanvasManager canvasManager;
    private Button thisButton;
    private Button ValidationButton;
    private Trial trial;
    private bool trialSelected = false;
    void Awake()
    {
        // Get already instantiated controllers and managers.
        canvasManager = GetComponentInParent<CanvasManager>();

        canvasManager.OnTrialSelected += OnTrialSelected;

        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnButtonClicked);
        thisButton.interactable = false;
    }
    private void Start()
    {
        Button[] buttons = transform.parent.GetComponentsInChildren<Button>();
        foreach (Button button in buttons) if (button.gameObject.name == "ValidateCamButton") ValidationButton = button;
        ValidationButton.onClick.AddListener(ValidationButtonPressed);
    }
    private void OnEnable()
    {
        if (trialSelected) StartCoroutine(WaitForNeededValues());
    }
    private void OnButtonClicked()
    {
        canvasManager.DisplayPopUp(CanvasType.CamCalibrationPopUp);
    }
    private void OnTrialSelected(Trial trial)
    {
        trialSelected = true;
        this.trial = trial;
        if (trial.CameraDone) thisButton.interactable = false;
        else if(!(trial.SizeDone && trial.SyncTimeDone && trial.RotationDone)) thisButton.interactable = false;
        else thisButton.interactable = true;
    }
    private IEnumerator WaitForNeededValues()
    {
        while (trial == null)
        {
            try { trial = canvasManager.GetTrial; }
            catch { trial = null; }
            yield return null;
        }

        if (trial.CameraDone) yield break;

        while (!(trial.SizeDone && trial.SyncTimeDone && trial.RotationDone)) yield return null;

        thisButton.interactable = true;

        StopCoroutine(WaitForNeededValues());
    }
    // Used to check wether or not we managed to feed correct values to the trial when the validation button was pressed.
    private void ValidationButtonPressed()
    {
        StartCoroutine(IsCameraDoneSuccesful());
    }
    // See for the next 5 frames if CameraDone turns to true. If it does, make this button uninteractable.
    private IEnumerator IsCameraDoneSuccesful()
    {
        int i = 0;
        while (!trial.CameraDone && i<5)
        {
            yield return 0;
            i++;
        }
        if(trial.CameraDone) thisButton.interactable = false;
    }
}
