using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Transform CoM;
    public float smoothSpeed;
    Vector3 Dist;
    int frameCount;
    // Start is called before the first frame update
    void Start()
    {
        Dist = transform.position - player.position;
       // frameCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPostion = player.position - player.forward * Dist.z + player.right * Dist.x + player.up * Dist.y;
        transform.position = Vector3.Lerp(transform.position, targetPostion, smoothSpeed);
        transform.LookAt(CoM);
    }
}
