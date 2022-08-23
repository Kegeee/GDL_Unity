using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetProjectionMatrix : MonoBehaviour
{
    Camera cam;

    public float clipF = 1000f; // Stands for clip far : the farthest side of the frustrum from the camera
    public float clipN = 0.3f; // stands for clip near : the nearest side of the frustrum from the camera

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        float i00 = 621.4731016831646f;
        float i02 = 964.83022727f;
        float i11 = 606.0916115661914f;
        float i12 = 547.77047973f;

        float fx = i00;
        float fy = i11;
        float W = 2 * i02;
        float H = 2 * i12;
        float Zn = clipN;
        float Zf = clipF;

        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = 2 * fx / W;
        m[0, 1] = 0;
        m[0, 2] = 0;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = 2 * fy / H;
        m[1, 2] = 0;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = -(Zf + Zn) / (Zf - Zn);
        m[2, 3] = -(2 * Zf * Zn) / (Zf - Zn);
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = -1;
        m[3, 3] = 0;

        cam.projectionMatrix = m;
    }
}
