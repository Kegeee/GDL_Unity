/*
 * this class inherits from ReturnButtonManager as it is essentially the same button but it returns to TrialMenu
 * instead of TrialSelection. It also loads the Starting scene.
 */

using UnityEngine.SceneManagement;

public class ReturnToMenuButtonManager : ReturnButtonManager
{
    protected override void OnButtonClicked()
    {
        canvasManager.SwitchCanvas(CanvasType.TrialMenu);
        SceneManager.LoadScene("StartingScene");
    }
}
