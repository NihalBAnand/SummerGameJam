using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject partyMember;
    public GameObject enemie;
    GameObject temp;
    public int enemies;
    public int ppl;
    int amount = 0;
    int xpos;
    int ypos;
    public int turn;
    public List<String> positions = new List<string>();
    public List<GameObject> players = new List<GameObject>();
    String posString;
    public int turnCycle = 0;

  
    void Start()
    {
       
        enemies = GlobalController.enemies;
        ppl = GlobalController.partymembers;
        turn = GlobalController.turn;
        Debug.Log(ppl);
        SpawnParty();
        SpawnEnemies();
        //scramble();
        players[0].GetComponent<Turn>().isTurn = true;
    }

    void SpawnEnemies()
    {
        amount = 0;
        while (amount < enemies)
        {
            xpos = Random.Range(-5, 0);
            ypos = Random.Range(-5, 0);
            posString = xpos.ToString() + "," + ypos.ToString();
            int xy = -1;
            while (positions.Contains(posString))
            {
                if (xy > 0)
                {
                    xpos += 2;
                }
                else
                {
                    ypos += 2;
                }
                posString = xpos.ToString() + "," + ypos.ToString();
                xy *= -1;
            }
            players.Add(Instantiate(enemie, new Vector3(xpos, ypos), Quaternion.identity));
            amount++;
            positions.Add(posString);
        }
        foreach (GameObject e in players)
        {
            e.tag = "player";
        }
    }

    // Update is called once per frame
    void Update()
    {
        endCondition();
        handleTurn();
    }

    void endCondition()
    {
        if(enemies <= 0 || ppl <= 0)
        {
            SceneManager.LoadScene("Assets/Scenes/Testing Ground 2.unity");
        }
    }

    void SpawnParty()
    {
        while (amount < ppl)
        {
            xpos = Random.Range(-5, 0);
            ypos = Random.Range(-5, 0);
            posString = xpos.ToString() + "," + ypos.ToString();
            int xy = -1;
            while (positions.Contains(posString))
            {
                if (xy > 0)
                {
                    xpos += 1;
                }
                else
                {
                    ypos += 1;
                }
                posString = xpos.ToString() + "," + ypos.ToString();
                xy *= -1;
            }
            temp = Instantiate(partyMember, new Vector3(xpos, ypos), Quaternion.identity);
            players.Add(temp);
            positions.Add(posString);
            amount++;

        }
    }
    void handleTurn()
    {


        if (GlobalController.turn >= players.Count)
        {
            GlobalController.turn = 0;
            turn = GlobalController.turn;
            GlobalController.turnCycle += 1;

        }

        else if (GlobalController.turn > turn || GlobalController.turn < turn)
        {
            players[turn].GetComponent<Turn>().isTurn = false;
            Debug.Log("turn done");
            players[GlobalController.turn].GetComponent<Turn>().isTurn = true;
            turn = GlobalController.turn;
        }
        players[GlobalController.turn].GetComponent<Turn>().isTurn = true;

    }
    void scramble()
    {
        for (int i = 1; i < players.Count; i++)
        { 
            GameObject temp = players[i];
            int randomIndex = Random.Range(i, players.Count);
            players[i] = players[randomIndex];
            players[randomIndex] = temp;
        }
    }
}
