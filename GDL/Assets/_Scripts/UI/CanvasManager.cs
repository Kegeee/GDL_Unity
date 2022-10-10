using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// Define public enumerators that will be useful for canvas and UI management.
public enum InputAxis
{
    X,
    Y,
    Z
}

public enum CanvasType
{
    TrialSelection,
    TrialMenu,
    Visualization,
    CameraCalibration,
    CamCalibrationPopUp
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
    private CanvasController currentPopUp;

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
                else if (OnTrialSelected == null) { Debug.LogWarning("No listeners added yet !"); return; }
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
            Debug.LogWarning("Could not get the desired trial.");
            return null;
        }
    }

    // Handling events.
    public event OnTrialSelectedDelegate OnTrialSelected;
    public delegate void OnTrialSelectedDelegate(Trial trial);

    void Awake()
    {
        // To make sure the menu manager is a singleton.
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // To make sure the menu data is persistent between scenes.
        DontDestroyOnLoad(gameObject);

        // Make sure all children - meaning all canvas - are turned on.
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // Get all canvas controller.
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();

        trials = GetAllTrials();
        Debug.Log("Trials is populated.");

        StartCoroutine(InitialiseMenu());
    }
    // This function is used to display a new canvas while turning off the previous one. 
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
    // Instantiate all trials from the .csv files in the resources folder, or from the .bin if it exists.
    private List<Trial> GetAllTrials()
    {
        DirectoryInfo d = new DirectoryInfo("Assets/Resources");

        FileInfo[] Files = d.GetFiles("*.csv");

        List<Trial> tempList = new List<Trial>();

        foreach (FileInfo file in Files)
        {
           /*   If a .bin version of this trial exists, store it in the list instead of the .csv.
            *    Uses code from all three following links :
            *    https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.surrogateselector?redirectedfrom=MSDN&view=net-6.0
            *    https://forum.unity.com/threads/vector3-is-not-marked-serializable.435303/
            *    https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/walkthrough-persisting-an-object-in-visual-studio
            */

            if (File.Exists(file.FullName.Remove(file.FullName.Length-4) + ".bin"))
            {
                Stream openFileStream = File.OpenRead(file.FullName.Remove(file.FullName.Length - 4) + ".bin");

                SurrogateSelector selector = new SurrogateSelector();
                // Inform the surrogate selector which surrogate it should use when encountering a Vector3.
                selector.AddSurrogate(typeof(Vector3), 
                    new StreamingContext(StreamingContextStates.All), 
                    new Vector3SerializationSurrogate());

                BinaryFormatter deserializer = new BinaryFormatter();
                deserializer.SurrogateSelector = selector;

                try
                {
                    Trial trial = (Trial)deserializer.Deserialize(openFileStream);
                    tempList.Add(trial);
                }
                catch(SerializationException e) 
                { 
                    tempList.Add(new Trial((int)Char.GetNumericValue(file.Name[file.Name.Length - 5])));
                    Debug.LogWarning("Could not deserialize : " + e 
                        + "\nTrial has been loaded from its .csv.");
                }
                openFileStream.Close();
            }
            else
            {
                tempList.Add(new Trial((int)Char.GetNumericValue(file.Name[file.Name.Length - 5])));
            }
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
    private IEnumerator InitialiseMenu()
    {
        while(!checkIfEveryObjectIsInstantiated(canvasControllerList)) yield return null;
        if (checkIfEveryObjectIsInstantiated(canvasControllerList))
        {
            SetSelectedTrialId = 2;
            TurnOffAllCanvas();
            SwitchCanvas(CanvasType.TrialSelection);
        }
        Debug.Log("Menu initialised.");
        StopCoroutine(InitialiseMenu()); 
    }
    // This function is used to display a pop up - meaning display a canvas without turning off the previous one.
    public void DisplayPopUp(CanvasType type)
    {
        if (!type.ToString().EndsWith("PopUp"))
        {
            Debug.LogWarning("Desired Canvas not a pop up!");
            return;
        }

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == type);
        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            currentPopUp = desiredCanvas;
        }
        else
        {
            Debug.LogWarning("The desired pop up not found!");
        }
    }
    // This function turns off the active pop up if it exists.
    public void turnOffPopUp()
    {
        if (currentPopUp != null) currentPopUp.gameObject.SetActive(false);
        else Debug.LogWarning("No active pop up !");
        currentPopUp = null;
    }
    // This function replace a trial in the trials list by a blank trial. The trial is selected by its ID.
    public void DeleteTrial(int trialIdToDelete)
    {
        for(int i = 0; i < trials.Count; i++) if (trials[i].TrialId == trialIdToDelete)
            {
                trials[i] = new Trial(trialIdToDelete);
            }
        SetSelectedTrialId = trialIdToDelete;
    }
}
