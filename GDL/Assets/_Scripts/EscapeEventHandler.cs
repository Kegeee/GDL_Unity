/*
 * This script listens to the "escape key pressed event" and closes the application when triggered.
 */

using UnityEngine;

public class EscapeEventHandler : MonoBehaviour
{    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit(); // ignored in editor, kept if we ever build the application itself.
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
