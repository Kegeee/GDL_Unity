using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InputAxis
{
    X,
    Y,
    Z
}

public class RotationIFManager : MonoBehaviour
{
    private CanvasController controller;
    private CanvasManager canvasManager;
    private TMP_InputField thisIF;
    private bool displayIF = true;
    private Trial selectedTrial;
    private TMP_Text thisText;
    private Button validationButton; 

    public InputAxis InputAxis;

    // Using start and not awake to make sure the event onClick of the button has already been subscribed by 
    // the button itself.
    void Start()
    {
        controller = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();
        thisIF = GetComponent<TMP_InputField>();
        thisText = GetComponentInChildren<TMP_Text>();
        canvasManager.OnTrialSelected += OnTrialUpdate;
        validationButton = transform.parent.parent.GetComponentInChildren<Button>();

        validationButton.onClick.AddListener(OnValidation);
    }
    private void OnTrialUpdate(Trial trial)
    {
        selectedTrial = trial;
        displayIF = !selectedTrial.RotationDone;
        if (displayIF)
        {
            thisIF.readOnly = false;
            thisIF.text = "0";
            thisText.color = Color.black;
        }
        else
        {
            thisIF.text = "";
            thisIF.readOnly = true;
            setTextToTrial();
            thisText.color = Color.green;
        }
    }
    private void OnValidation()
    {
        thisIF.text = "";
        thisIF.readOnly = true;
        setTextToTrial();
        thisText.color = Color.green;
    }
    // This function simply set the text to be the one needed depending on the axis and the trial.
    private void setTextToTrial()
    {
        if (InputAxis == InputAxis.X) thisText.SetText(System.Convert.ToString(selectedTrial.RotationOffset.x));
        if (InputAxis == InputAxis.Y) thisText.SetText(System.Convert.ToString(selectedTrial.RotationOffset.y));
        if (InputAxis == InputAxis.Z) thisText.SetText(System.Convert.ToString(selectedTrial.RotationOffset.z));
    }
}
