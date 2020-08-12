using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class battleBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public string levelName = "yeet1";
    public GameObject temp;
    public GameObject enemie;
    public GameObject StaticObj;
    public GameObject partyPos;
    
    void Start()
    {
        levelName = "01a";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            parseToFile();
        }
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            parseText();
        }
    }

    void parseToFile()
    {
        string path = Application.dataPath + "/Battles/" + levelName + ".txt";
        File.WriteAllText(path, "");
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.tag != "MainCamera")
            {
                string name = go.tag;
                if (name == "Untagged")
                {
                    name = "obj";
                }
                string strpos;
                Vector3 pos = go.transform.position;
                strpos = pos.x.ToString() + "," + pos.y.ToString();
                File.AppendAllText(path, name + "," + strpos + "\n");
            }

        }
    }
    void parseText()
    {
        StreamReader reader = File.OpenText(Application.dataPath + "/Battles/" + GlobalController.battle + ".txt");
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] items = line.Split(',');
            string type = items[0];
            float xpos = float.Parse(items[1]);
            float ypos = float.Parse(items[2]);
            if (type == "enemy")
            {

                temp =(Instantiate(enemie, new Vector3(xpos, ypos), Quaternion.identity));
                

            }
            else if (type == "obj")
            {
                temp =(Instantiate(StaticObj, new Vector3(xpos, ypos), Quaternion.identity));
            }
            else if (type == "partyPosition")
            {
               

                
            }
        }
    }
}
