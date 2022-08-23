using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    GameObject environnement;
    GameObject workspace;
    void Start()
    {
        environnement = transform.GetChild(0).gameObject;
        workspace = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (environnement.activeSelf)
            {
                environnement.SetActive(false);
                workspace.SetActive(true);
            }
            else
            {
                environnement.SetActive(true);
                workspace.SetActive(false);
            }
        }
    }
}
