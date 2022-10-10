using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RedoCalibrationButtonManager : MonoBehaviour
{
    private CanvasController controller;
    private Button thisButton;
    private Image thisBackground;

    void Awake()
    {
        controller = GetComponentInParent<CanvasController>();
        thisButton = GetComponent<Button>();

        thisButton.onClick.AddListener(OnButtonClicked);

        thisBackground = GetComponent<Image>();

        // Subscribe to the scene loaded event from the scene manager.
        SceneManager.sceneLoaded += OnSceneLoaded;

        controller.allUIs[transform.GetSiblingIndex()] = true;
    }
    private void Start()
    {
        // Make sure the button stays hidden while calibration is not complete.
        hideButton(true);
    }
    private void OnButtonClicked()
    {
        SceneManager.LoadScene("initialiseAngleGDL");
        hideButton(true);
    }
    // When the event calibration finished is raised we stop hidding the button.
    private void OnCalibrationFinished(Vector3 result)
    {
        hideButton(false);
    }
    // This handler is used to subscribe to the calibration finished event as soon as possible - meaning right after the 
    // calibration scene has finished loading.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "initialiseAngleGDL") return;
        else GameObject.Find("Head cam").GetComponent<GazeCalibration>().OnCalibrationFinished += OnCalibrationFinished;
    }
    // If set to true, this function hides the button. If set to false, it shows the button.
    private void hideButton(bool hide)
    {
        thisBackground.enabled = !hide;
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(!hide);
    }
}
