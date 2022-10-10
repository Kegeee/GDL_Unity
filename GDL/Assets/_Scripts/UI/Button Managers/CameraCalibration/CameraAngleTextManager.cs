/*
 * This script sets the text so that we may know in real time the calibration camera euler angle.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CameraAngleTextManager : MonoBehaviour
{
    private CanvasController controller;
    private TMP_Text thisText;
    private Camera cam;

    void Awake()
    {
        controller = GetComponentInParent<CanvasController>();
        thisText = GetComponent<TMP_Text>();
        cam = null;

        controller.allUIs[transform.GetSiblingIndex()] = true;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "initialiseAngleGDL" && cam == null)
        {
            try
            {
                cam = GameObject.Find("Head cam").GetComponent<Camera>();                
            }
            catch
            {
                cam = null;
            }
        }
        if (cam != null)
        {
            float displayX = cam.transform.localEulerAngles.x > 180 ? 
                cam.transform.localEulerAngles.x - 360 : cam.transform.localEulerAngles.x;
            float displayY = cam.transform.localEulerAngles.y > 180 ? 
                cam.transform.localEulerAngles.y - 360 : cam.transform.localEulerAngles.y;
            float displayZ = cam.transform.localEulerAngles.z > 180 ? 
                cam.transform.localEulerAngles.z - 360 : cam.transform.localEulerAngles.z;
            thisText.SetText($"({Math.Round(displayX, 2)}," +
            $"{Math.Round(displayY, 2)}," +
            $"{Math.Round(displayZ, 2)})");
        }
    }
}
