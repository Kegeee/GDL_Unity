using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalWalking : MonoBehaviour
{
    void FixedUpdate()
    {
        RaycastHit hit;
        if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), -Vector3.up, out hit))
            return;
        transform.position = new Vector3(transform.position.x, transform.position.y + 2f - hit.distance, transform.position.z);
    }
}