using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButtonManager : MonoBehaviour
{
    private CanvasController controller;
    private CanvasManager canvasManager;
    private Button thisButton;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnButtonClicked);

        controller.allUIs[transform.GetSiblingIndex()] = true;
    }
    void OnButtonClicked()
    {
        canvasManager.SwitchCanvas(CanvasType.TrialSelection);
    }
}
