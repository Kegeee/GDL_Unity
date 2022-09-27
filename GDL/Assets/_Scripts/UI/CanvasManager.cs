using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System;


public enum CanvasType
{
    TrialSelection,
    TrialMenu,
    Visualization
}

public class CanvasManager : MonoBehaviour
{
    // Needed data for trial visualisation
    public static List<Trial> trials;
    private static int selectedTrialId;

    // Needed variables for canvas management
    public static CanvasManager instance;
    private List<CanvasController> canvasControllerList;
    private CanvasController lastActiveCanvas;

    // Getter and setter
    public CanvasManager Instance
    {
        get { return instance; }
    }
    public int SetSelectedTrialId
    {
        set 
        {
            // If something tries to set the currently selected trial ID, we allow it but afterward we trigger the appropriate
            // events.
            selectedTrialId = value;

            foreach (Trial trial in trials)
            {
                if (OnTrialSelected != null && value == trial.TrialId) OnTrialSelected(trial);
                else if(OnTrialSelected == null) { Debug.LogWarning("No listeners added yet !"); return; }
            }
        }
    }
    public List<Trial> Trials
    {
        get { return trials; }
    }
    public Trial GetTrial
    {
        get 
        {            
            foreach (Trial trial in trials)
            {
                if (selectedTrialId == trial.TrialId) return trial;
            }
            Debug.LogWarning("Could not get the selected trial.");
            return null;
        }
    }

    // Handling events.
    public event OnTrialSelectedDelegate OnTrialSelected;
    public delegate void OnTrialSelectedDelegate(Trial trial);

    // Start is called before the first frame update
    void Awake()
    {
        // To make sure the menu manager is a singleton.
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // To make sure the menu data is persistent.
        DontDestroyOnLoad(gameObject);

        // Get all canvas controller.
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();

        trials = GetAllTrials();
        Debug.Log("Trials is populated.");

        StartCoroutine(InitialiseMenu());
    }

    public void SwitchCanvas(CanvasType type)
    {
        if(lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == type);
        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;
        }
        else
        {
            Debug.LogWarning("The desired canvas not found!");
        }
    }
    public void TurnOffAllCanvas()
    {
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
    }
    private List<Trial> GetAllTrials()
    {
        DirectoryInfo d = new DirectoryInfo("Assets/Resources");

        FileInfo[] Files = d.GetFiles("*.csv");

        List<Trial> tempList = new List<Trial>();

        foreach (FileInfo file in Files)
        {
            tempList.Add(new Trial((int)Char.GetNumericValue(file.Name[file.Name.Length - 5])));
        }
        return tempList;
    }
    // This function checks if all objects in the different canvas are instantiated or not,
    // return true if they are, false if they aren't.
    private bool checkIfEveryObjectIsInstantiated(List<CanvasController> listOfCanvas)
    {
        bool result = true;
        foreach (CanvasController canvas in listOfCanvas)
        {
            int index = 0;
            while (result && index < canvas.allUIs.Length)
            {
                result = canvas.allUIs[index];
                index++;
            }
        }
        return result;
    }
    // This coroutine initialise the menu by checking that every objects in the UI is correctly instantiated
    // before selecting the right trial and the rigth canvas.
    IEnumerator InitialiseMenu()
    {
        while(!checkIfEveryObjectIsInstantiated(canvasControllerList)) yield return null;
        // When all objects are properly initialised, we select the correct canvas to display and the correct trial.
        if (checkIfEveryObjectIsInstantiated(canvasControllerList))
        {
            SetSelectedTrialId = 2;
            TurnOffAllCanvas();
            SwitchCanvas(CanvasType.TrialSelection);
        }
        StopCoroutine(InitialiseMenu()); // Therefore the menu is initialised and we don't need the coroutine anymore.
    }
}
