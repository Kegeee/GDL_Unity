/*
 * This script is only used to make sure every objects in UI are correctly instantiated in the canvas.
 * It turns "true" an allocated boolean in its canvas controller.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultUIManager : MonoBehaviour
{
    private CanvasController controller;
    void Awake()
    {
        // Get already instantiated controllers and managers.
        controller = GetComponentInParent<CanvasController>();

        try
        {
            controller.allUIs[transform.GetSiblingIndex()] = true; // Set to true the corresponding boolean of this object.
        }
        catch
        {
            Debug.LogWarning("There is no array to attach to.");
        }
    }
}
