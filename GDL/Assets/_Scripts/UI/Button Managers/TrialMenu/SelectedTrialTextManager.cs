using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
/*
 * Listens to changes in "Trial selected" and updates the displayed text accordingly.
 * */
public class SelectedTrialTextManager : MonoBehaviour
{
    private CanvasController controller;
    private CanvasManager canvasManager;
    private TMP_Text thisText;
    // Start is called before the first frame update
    void Awake()
    {
        canvasManager = GetComponentInParent<CanvasManager>();
        canvasManager.Instance.OnTrialSelected += SetText;
        controller = GetComponentInParent<CanvasController>();
        thisText = GetComponent<TMP_Text>();

        controller.allUIs[transform.GetSiblingIndex()] = true;
    }
    private void SetText(Trial e)
    {
        thisText.SetText($"Trial Selected : {e.TrialId}");
    }
}
