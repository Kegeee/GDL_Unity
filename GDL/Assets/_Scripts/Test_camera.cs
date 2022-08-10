using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Test_camera : MonoBehaviour
{
    /*
    // define values of the intrinsic matrix - self made calibration from 07/15/2022
    float i00 = 793.8052697386686f;
    float i02 = 953.2237035923064f;
    float i11 = 792.3104221704713f;
    float i12 = 572.5036513432223f;
    // values of the virtual camera used for undistortion
    float i00 = 621.4731016831646f;
    float i02 = 1193.9113626217502f;
    float i11 = 606.0916115661914f;
    float i12 = 519.5085285404007f;
    */

    // to see what clip far and clip near are all about
    float clipF = 1000f;
    float clipN = 0.3f;

    public float fov = 180f;
    public float aspect = 1;

    public Camera cam;

    Matrix4x4 m;

    public void update()
    {
        /*
        float fx = i00;
        float fy = i11;
        float W = 2 * i02;
        float H = 2 * i12;
        */
        float Zn = clipN;
        float Zf = clipF;
        //float Zn = cam.nearClipPlane;
        //float Zf = cam.farClipPlane;

        // Matrix4x4 m = PerspectiveOffCenter(-0.2f, 0.2f, 0.2f, -0.2f, Zn, Zf);

        // define projection matrix (openGL) based on intrinsic matrix (openCV), see :
        // http://www.info.hiroshima-cu.ac.jp/~miyazaki/knowledge/teche0092.html

        m = new Matrix4x4();
        //m[0, 0] = 2 * fx / W;
        m[0, 0] = 1f/(Mathf.Tan(fov/2f));
        m[0, 1] = 0;
        m[0, 2] = 0;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = m[0,0] / aspect ;
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
    // not used, kept for testing purposes 
    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }
}
