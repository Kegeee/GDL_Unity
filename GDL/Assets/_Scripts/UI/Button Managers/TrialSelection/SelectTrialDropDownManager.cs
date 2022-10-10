using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SelectTrialDropDownManager : MonoBehaviour
{
    private CanvasController controller;
    private CanvasManager canvasManager;
    private TMP_Dropdown thisDropDown;
    // The dropdown is initialized in start to make sure the trials list in the manager is populated.
    void Start()
    {
        // Get already instantiated controllers and managers.
        controller = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();
        thisDropDown = GetComponent<TMP_Dropdown>();
        
        // This clears default options of the DropDown.
        thisDropDown.ClearOptions();
        
        PopulateOptions();

        // Adds the listener.
        thisDropDown.onValueChanged.AddListener( delegate
        {
            OnValueChanged();
        }
        );
        //thisDropDown.value = 0;
        thisDropDown.RefreshShownValue();

        // Inform the controller that the dropdown is instantiated.
        try
        {
            controller.allUIs[transform.GetSiblingIndex()] = true; // Set to true the corresponding boolean of this object.
        }
        catch
        {
            Debug.LogWarning("There is no array to attach to.");
        }
    }
    // Keeps the manager updated on which trial is selected.
    void OnValueChanged()
    {
        int captionLength = thisDropDown.captionText.text.Length;
        // Kept for debug purposes.
        //Debug.Log($"Nouveau trial choisit = {Char.GetNumericValue(thisDropDown.captionText.text[captionLength - 1])}");
        canvasManager.SetSelectedTrialId = (int)Char.GetNumericValue(thisDropDown.captionText.text[captionLength - 1]);
    }
    public void PopulateOptions()
    {
        // Set options based on fetched trials in the canvas manager.
        foreach (Trial t in CanvasManager.trials)
        {
            thisDropDown.options.Add(new TMP_Dropdown.OptionData(t.TrialFile.Substring(0, t.TrialFile.Length-4)));
        }
    }
}
