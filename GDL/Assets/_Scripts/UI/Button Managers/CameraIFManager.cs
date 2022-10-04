using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CameraIFManager : InputFieldSkeleton
{
    // To know which axis this script is suposed to handle. Has to be set in the editor though.
    public InputAxis InputAxis;

    private Button validationButton;
    private CanvasController controller;


    // Using start and not awake to make sure the event onClick of the button has already been subscribed by 
    // the button itself.
    protected override void Awake()
    {
    }
    void Start()
    {
        controller = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();
        thisInputField = GetComponent<TMP_InputField>();
        thisText = GetComponentInChildren<TMP_Text>();
        canvasManager.OnTrialSelected += OnTrialUpdate;
        validationButton = transform.parent.parent.GetComponentInChildren<Button>();

        validationButton.onClick.AddListener(OnValidation);
    }
    // Turning off update on this IF because it shoud not wait for a key to enter at runtime as the validation button is here for that.
    override protected void Update()
    {
    }
    private void OnValidation()
    {
        if (TrialDone())
        {
            thisInputField.text = "";
            thisInputField.readOnly = true;
            thisText.SetText(TrialText());
            thisText.color = Color.green;
        }
    }
    // This function simply set the text to be the one needed depending on the axis and the trial.
    override protected string TrialText()
    {
        if (InputAxis == InputAxis.X) return System.Convert.ToString(selectedTrial.CameraOffset.x);
        if (InputAxis == InputAxis.Y) return System.Convert.ToString(selectedTrial.CameraOffset.y);
        if (InputAxis == InputAxis.Z) return System.Convert.ToString(selectedTrial.CameraOffset.z);
        Debug.Log("Should not be possible. See Camera IF manager script.");
        return "Not possible. See CameraIF manager script.";
    }
    protected override bool TrialDone()
    {
        return selectedTrial.CameraDone;
    }
    // As of now, this method is not needed.
    protected override void SetTrial()
    {
        throw new System.NotImplementedException();
    }
}
