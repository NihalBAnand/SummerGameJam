using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class battleBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public string levelName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            parseToFile();
        }
    }

    void parseToFile()
    {

    }
}
