using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosePopUpButtonManager : MonoBehaviour
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

        // Informs the controller that this UI element is prepared.
        controller.allUIs[transform.GetSiblingIndex()] = true;
    }
    private void OnButtonClicked()
    {
        canvasManager.turnOffPopUp();
    }
}
