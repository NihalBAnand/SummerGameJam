using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.IO;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject partyMember;
    public GameObject enemie;
    public GameObject StaticObj;
    GameObject temp;
    public int enemies;
    public int ppl;
    public int ranged;
    int amount = 0;
    int xpos;
    int ypos;
    public int turn;
    public List<string> positions = new List<string>();
    public List<GameObject> players = new List<GameObject>();
    public Sprite rangedSprite;
    public Sprite healerSprite;
    String posString;
    public int turnCycle = 0;
    public int globTurn;
    public int objs;
    public int healers;

    public List<GameObject> parsedObjs = new List<GameObject>();
    public List<GameObject> parsedPlayers = new List<GameObject>();
    public List<GameObject> parsedEnemies = new List<GameObject>();

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
                parsedEnemies.Add(Instantiate(enemie, new Vector3(xpos, ypos), Quaternion.identity));
            }
            else if (type == "obj")
            {
                parsedObjs.Add(Instantiate(StaticObj, new Vector3(xpos, ypos), Quaternion.identity));
            }
            else if (type == "partyPosition")
            {
                amount = 0;
                while (amount < ppl)
                {
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
                    if (amount < ranged)
                    {
                        temp.GetComponent<SpriteRenderer>().sprite = rangedSprite;
                        temp.GetComponent<MeleePlayerBattleController>().ranged = true;
                    }
                    else if (amount < (ranged + healers))
                    {
                        temp.GetComponent<MeleePlayerBattleController>().healer = true;
                        temp.GetComponent<SpriteRenderer>().sprite = healerSprite;
                    }
                    players.Add(temp);
                    positions.Add(posString);
                    amount++;

                }
            }
        }
    }
  
    void Start()
    {
        
        enemies = GlobalController.enemies;
        ppl = GlobalController.partymembers;
        turn = GlobalController.turn;
        ranged = GlobalController.rangedPlayers;
        objs = GlobalController.objs;
        healers = GlobalController.healers;
        ppl += ranged;
        ppl += healers;
        Debug.Log(ppl);
        SpawnParty();
        SpawnEnemies();
        //scramble();
        spawnObjs();
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
        globTurn = GlobalController.turn;
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
        amount = 0;
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
            if (amount < ranged)
            {
                temp.GetComponent<SpriteRenderer>().sprite = rangedSprite;
                temp.GetComponent<MeleePlayerBattleController>().ranged = true;
            }
            else if (amount < (ranged + healers))
            {
                temp.GetComponent<MeleePlayerBattleController>().healer = true;
                temp.GetComponent<SpriteRenderer>().sprite = healerSprite;
            }
            players.Add(temp);
            positions.Add(posString);
            amount++;

        }
    }
    void spawnObjs()
    {
        for(int i =0; i<objs; i++)
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

            temp = Instantiate(StaticObj, new Vector3(xpos, ypos), Quaternion.identity);
        }
        positions.Add(posString);
    }
    void handleTurn()
    {


        if (GlobalController.turn >= players.Count)
        {
            GlobalController.turn = 0;
            turn = GlobalController.turn;
            GlobalController.turnCycle += 1;
        }

        else if (GlobalController.turn != turn)
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
        for (int i = 0; i < players.Count; i++)
        { 
            GameObject temp = players[i];
            int randomIndex = Random.Range(i, players.Count);
            players[i] = players[randomIndex];
            players[randomIndex] = temp;
        }
    }
}
