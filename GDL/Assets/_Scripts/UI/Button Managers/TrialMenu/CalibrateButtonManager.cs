using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalibrateButtonManager : MonoBehaviour
{
    private CanvasManager canvasManager;
    private Button thisButton;
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
        if(!(trial.SizeDone && trial.SyncTimeDone && trial.RotationDone)) thisButton.interactable = false;
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

        while (!(trial.SizeDone && trial.SyncTimeDone && trial.RotationDone)) yield return null;

        thisButton.interactable = true;

        StopCoroutine(WaitForNeededValues());
    }
}
