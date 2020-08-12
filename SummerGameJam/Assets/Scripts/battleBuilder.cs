using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class battleBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public string levelName = "yeet1";
    void Start()
    {
        levelName = "yeet1";
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
        string path = Application.dataPath + "/Battles/" + levelName + ".txt";
        File.WriteAllText(path, "");
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            string name = go.tag;
            if(name == "Untagged")
            {
                name= "obj";
            }
            string strpos;
            Vector3 pos = go.transform.position;
            strpos = pos.x.ToString() + "," + pos.y.ToString();
            File.AppendAllText(path, name+"," + strpos + "\n");
        }
            

    }
}
