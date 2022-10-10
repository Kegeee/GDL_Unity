using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveTrialButtonManager : MonoBehaviour
{
    private CanvasController canvasController;
    private CanvasManager canvasManager;
    private Trial trial;
    private Button button;
    private bool trialSelected;
    void Awake()
    {
        canvasController = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();
        button = GetComponent<Button>();

        canvasManager.OnTrialSelected += OnTrialSelected;
        button.onClick.AddListener(SaveTrial);

        canvasController.allUIs[transform.GetSiblingIndex()] = true;
    }
    private void OnEnable()
    {
        if (trialSelected)
        {
            if (File.Exists("Assets/Resources/" + trial.TrialFile.Remove(trial.TrialFile.Length - 4) + ".bin")) button.interactable = false;
            else button.interactable = true;
        }
    }
    /*
     * Serializes the selected trial. Uses code taken from these three links :
     *
     * https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.surrogateselector?redirectedfrom=MSDN&view=net-6.0
     * https://forum.unity.com/threads/vector3-is-not-marked-serializable.435303/
     * https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/walkthrough-persisting-an-object-in-visual-studio
     */
    private void SaveTrial()
    {
        Stream SaveFileStream = File.Create("Assets/Resources/" + trial.TrialFile.Remove(trial.TrialFile.Length - 4) + ".bin");

        SurrogateSelector selector = new SurrogateSelector();
        selector.AddSurrogate(typeof(Vector3),
            new StreamingContext(StreamingContextStates.All),
            new Vector3SerializationSurrogate());

        BinaryFormatter serializer = new BinaryFormatter();
        serializer.SurrogateSelector = selector;

        try
        {
            serializer.Serialize(SaveFileStream, trial);
            Debug.Log("Serialization of trial successful.");
            button.interactable = false;
        }
        catch (SerializationException e){ Debug.LogWarning("Could not serialize : " + e); }

        SaveFileStream.Close();
    }
    private void OnTrialSelected(Trial trial)
    {
        trialSelected = true;
        this.trial = trial;
    }
}
