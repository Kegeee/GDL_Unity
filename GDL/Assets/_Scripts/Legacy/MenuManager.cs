/*
 * ========================================================
 * 
 * 
 *                      Deprecated!!!
 *              Delete this when UI is finished.
 * 
 * 
 * ========================================================
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public static List<Trial> trials = new List<Trial>();

    public Canvas mainMenu;
    public TMP_Dropdown trialDropDown;
    public Button trialSelectedButton;

    public Canvas trialMenu;
    public Button returnButton;
    public TMP_Text trialClue;

    private int selectedTrial;

    private void Awake()
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
        // Initialise the menu.
        mainMenu.enabled = true;
        trialMenu.enabled = false;

        // Initialise the main menu dropdown.
        trialDropDown.ClearOptions();
        trialDropDown.onValueChanged.AddListener(delegate
        {
            int captionLength = trialDropDown.captionText.text.Length;
            Debug.Log($"Nouveau trial choisit = {Char.GetNumericValue(trialDropDown.captionText.text[captionLength-1])}");
            selectedTrial = (int)Char.GetNumericValue(trialDropDown.captionText.text[captionLength - 1]);
        });
        // Initialise the main menu trial selected button.
        trialSelectedButton.onClick.AddListener(delegate
        {
            trialClue.text = "Selected trial : " + selectedTrial;
            trialMenu.enabled = true;
            mainMenu.enabled = false;
        });
        // Initialise the return to trial selection button.
        returnButton.onClick.AddListener(delegate
        {
            mainMenu.enabled = true;
            trialMenu.enabled = false;
        });
    }
    private void Start()
    {
        DirectoryInfo d = new DirectoryInfo("Assets/Resources"); 

        FileInfo[] Files = d.GetFiles("*.csv");

        foreach (FileInfo file in Files)
        {
            trials.Add(new Trial((int)Char.GetNumericValue(file.Name[file.Name.Length - 5])));
            trialDropDown.options.Add(new TMP_Dropdown.OptionData(file.Name.Substring(0,file.Name.Length-4)));
        }
        trialDropDown.RefreshShownValue();
        selectedTrial = (int)Char.GetNumericValue(trialDropDown.options[0].text[trialDropDown.options[0].text.Length - 1]);
    }
}
