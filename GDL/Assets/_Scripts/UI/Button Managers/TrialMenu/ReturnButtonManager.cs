using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButtonManager : MonoBehaviour
{
    protected CanvasController controller;
    protected CanvasManager canvasManager;
    protected Button thisButton;
    // Start is called before the first frame update
    protected void Awake()
    {
        controller = GetComponentInParent<CanvasController>();
        canvasManager = GetComponentInParent<CanvasManager>();
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnButtonClicked);

        controller.allUIs[transform.GetSiblingIndex()] = true;
    }
    protected virtual void OnButtonClicked()
    {
        canvasManager.SwitchCanvas(CanvasType.TrialSelection);
    }
}
