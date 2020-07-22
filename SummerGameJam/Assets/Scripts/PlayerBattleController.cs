using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerBattleController : MonoBehaviour
{
    int speed = 600;
    int moved = 0;

    public bool enemyAdj = false;

    public Text moveText;
    public Text alert;

    public List<string> enemyDirections = new List<string>();

    public List<GameObject> enemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("enemy"))
        {
            enemies.Add(obj);
        }
        alert.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moved < speed)
            Movement();
        DetectObjects();
        UpdateUI();
    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!enemyDirections.Contains("L"))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!enemyDirections.Contains("R"))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!enemyDirections.Contains("U"))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!enemyDirections.Contains("D"))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                moved++;
            }
        }
    }

    void UpdateUI()
    {
        moveText.text = "Movement left: " + (speed - moved);
        alert.enabled = enemyAdj;
    }

    void DetectObjects()
    {
        foreach (GameObject e in enemies)
        {
            if ((e.transform.position.x + 1 == gameObject.transform.position.x || e.transform.position.x - 1 == gameObject.transform.position.x) && (e.transform.position.y == gameObject.transform.position.y))
            {
                enemyAdj = true;
            }
            else if (e.transform.position.x == gameObject.transform.position.x && (e.transform.position.y + 1 == gameObject.transform.position.y || e.transform.position.y - 1 == gameObject.transform.position.y))
            {
                enemyAdj = true;
            }
            else
            {
                enemyAdj = false;
            }
            if (enemyAdj)
            {
                if (gameObject.transform.position.y > e.transform.position.y)
                {
                    enemyDirections.Add("D");
                }
                else if (enemyDirections.Contains("D"))
                {
                    enemyDirections.Remove("D");
                }

                if (gameObject.transform.position.y < e.transform.position.y)
                {
                    enemyDirections.Add("U");
                }
                else if (enemyDirections.Contains("U"))
                {
                    enemyDirections.Remove("U");
                }

                if (gameObject.transform.position.x > e.transform.position.x)
                {
                    enemyDirections.Add("L");
                }
                else if (enemyDirections.Contains("L"))
                {
                    enemyDirections.Remove("L");
                }

                if (gameObject.transform.position.x < e.transform.position.x)
                {
                    enemyDirections.Add("R");
                }
                else if (enemyDirections.Contains("R"))
                {
                    enemyDirections.Remove("R");
                }
            }
            else
            {
                enemyDirections.Clear();
            }
        }
    }
}
