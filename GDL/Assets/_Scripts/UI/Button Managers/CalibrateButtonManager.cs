using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalibrateButtonManager : MonoBehaviour
{
    private CanvasManager canvasManager;
    private Button thisButton;
    void Awake()
    {
        // Get already instantiated controllers and managers.
        canvasManager = GetComponentInParent<CanvasManager>();

        thisButton = GetComponent<Button>();

        thisButton.onClick.AddListener(OnButtonClicked);
    }
    private void OnButtonClicked()
    {
        canvasManager.DisplayPopUp(CanvasType.CamCalibrationPopUp);
    }
}
