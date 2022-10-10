using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VisualisationButtonManager : MonoBehaviour
{
    private CanvasController controller;
    private CanvasManager canvasManager;
    private Button thisButton;
    private Trial trial = null;
    private bool trialSelected = false;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();

        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnButtonClicked);
        thisButton.interactable = false; // Should not be able to launch visualisation if the trial is not ready yet.

        canvasManager.OnTrialSelected += OnTrialSelected;

        // Informs the controller that this UI element is prepared.
        controller.allUIs[transform.GetSiblingIndex()] = true;
    }
    private void OnEnable()
    {
        if(trialSelected) StartCoroutine(WaitForTrialToBeReady());
    }
    private void OnButtonClicked()
    {
        canvasManager.SwitchCanvas(CanvasType.Visualization);
        SceneManager.LoadScene("GDL_Main");
    }
    private IEnumerator WaitForTrialToBeReady()
    {
        while (trial == null)
        {
            try { trial = canvasManager.GetTrial; }
            catch { trial = null; }
            yield return null;
        }

        while (!(trial.SizeDone && trial.SyncTimeDone && trial.RotationDone && trial.CameraDone)) yield return null;

        thisButton.interactable = true;

        StopCoroutine(WaitForTrialToBeReady());
    }
    private void OnTrialSelected(Trial trial)
    {
        trialSelected = true;
        this.trial = trial;
        if (!(trial.SizeDone && trial.SyncTimeDone && trial.RotationDone && trial.CameraDone)) thisButton.interactable = false;
        else thisButton.interactable = true;
    }
}
