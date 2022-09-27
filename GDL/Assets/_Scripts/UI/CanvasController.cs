using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanvasController : MonoBehaviour
{
    public CanvasType canvasType = CanvasType.TrialSelection;

    private bool[] m_allUIs = new bool[25]; 
    // Instantiate allUIs to an enormous anmount of object so that all objects
    // may make change to it even though the canvasControllet is not awake yet.
    public bool[] allUIs
    {
        get { return m_allUIs; }
        set 
        {
            m_allUIs = value;
        }
    }

    private void Awake()
    {
        // Resize allUIs to the correct size.
        bool[] tempUIs = allUIs;
        // By default in C# all bool values are false.
        Array.Resize(ref tempUIs, transform.childCount);
        allUIs = tempUIs;
    }
}
