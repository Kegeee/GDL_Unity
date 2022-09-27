using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VisualisationButtonManager : MonoBehaviour
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

    private void OnButtonClicked()
    {
        canvasManager.SwitchCanvas(CanvasType.Visualization);
        SceneManager.LoadScene("GDL_Main");
    }
}
