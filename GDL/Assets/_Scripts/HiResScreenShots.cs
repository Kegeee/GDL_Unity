using UnityEngine;
using System.Collections;

public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 1920;
    public int resHeight = 1080;

    public float[] screenTime;
    public string screenPath = "C:\\Users\\bilel\\Desktop\\result\\quickMovementExample";

    private Camera cam;
    private int index = 0;

    public static string ScreenShotName(int width, int height, string screenPath)
    {
        return string.Format("{0}_{1}x{2}_{3}.png",
                             screenPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
    }


    void FixedUpdate()
    {
        string plop = index < screenTime.Length ? "true" : "false";
        if (index < screenTime.Length)
        {
            if(Time.time >= screenTime[index])
            {
                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                cam.targetTexture = rt;
                Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                cam.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
                cam.targetTexture = null;
                RenderTexture.active = null; // JC: added to avoid errors
                Destroy(rt);
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = ScreenShotName(resWidth, resHeight,screenPath);
                System.IO.File.WriteAllBytes(filename, bytes);
                Debug.Log(string.Format("Took screenshot to: {0}", filename));
                index++;
            }
        }
    }
}