using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DeleteTrialButtonManager : MonoBehaviour
{
    private CanvasController canvasController;
    private CanvasManager canvasManager;
    private Trial trial;
    private Button button;
    private Button saveTrialButton;
    private bool trialSelected = false;
    void Awake()
    {
        canvasController = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();
        button = GetComponent<Button>();

        canvasManager.OnTrialSelected += OnTrialSelected;
        button.onClick.AddListener(DeleteTrial);

        canvasController.allUIs[transform.GetSiblingIndex()] = true;
    }
    private void Start()
    {
        saveTrialButton = GameObject.Find("SaveTrialButton").GetComponent<Button>();
        saveTrialButton.onClick.AddListener(OnSaveTrial);
    }
    private void OnEnable()
    {
        if (trialSelected)
        {
            if (!File.Exists("Assets/Resources/" + trial.TrialFile.Remove(trial.TrialFile.Length - 4) + ".bin")) button.interactable = false;
            else button.interactable = true;
        }
    }
    // Delete the .bin file of this trial if the user wants to. If deleting was succesful, asks the canvas manager to reload this canvas
    // after deleting the appropriate trial object.
    private void DeleteTrial()
    {
        try
        {
            if (File.Exists("Assets/Resources/" + trial.TrialFile.Remove(trial.TrialFile.Length - 4) + ".bin"))
            {
                File.Delete("Assets/Resources/" + trial.TrialFile.Remove(trial.TrialFile.Length - 4) + ".bin");
                button.interactable = false;
                Debug.Log("File deleted.");
                CanvasManager.instance.DeleteTrial(trial.TrialId);
                
                CanvasManager.instance.SwitchCanvas(CanvasType.TrialMenu);
            }
            else Debug.LogWarning("File not found");
        }
        catch (IOException ioExp)
        {
            Debug.LogWarning("Could not delete the trial : " + ioExp.Message);
        }
    }
    private void OnTrialSelected(Trial trial)
    {
        trialSelected = true;
        this.trial = trial;
    }
    private void OnSaveTrial()
    {
        if (!File.Exists("Assets/Resources/" + trial.TrialFile.Remove(trial.TrialFile.Length - 4) + ".bin")) button.interactable = false;
        else button.interactable = true;
    }
}
