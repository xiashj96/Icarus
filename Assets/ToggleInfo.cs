using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInfo : MonoBehaviour
{
    GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (canvas.activeSelf)
            {
                canvas.SetActive(false);
            }
            else
            {
                canvas.SetActive(true);
            }
        }
    }
}
