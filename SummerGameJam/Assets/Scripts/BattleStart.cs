using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class BattleStart : MonoBehaviour
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
  
    void Start()
    {
       
        enemies = GlobalController.enemies;
        ppl = GlobalController.partymembers;
        turn = GlobalController.turn;
        Debug.Log(ppl);
        SpawnParty();
        SpawnEnemies();
        try
        {
            players[0].GetComponent<MeleePlayerBattleController>().isTurn = true;
        }
        catch(Exception)
        {
            players[0].GetComponent<EnemyBattleScript>().isTurn = true;
        }

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
                    xpos += 1;
                }
                else
                {
                    ypos += 1;
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
        handleTurn();
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

        }
        else if (GlobalController.turn > turn || GlobalController.turn < turn)
        {
            
            if (players[turn].GetComponent<MeleePlayerBattleController>() != null)
            {
                players[turn].GetComponent<MeleePlayerBattleController>().isTurn = false;
            }
            else
            {
                players[turn].GetComponent<EnemyBattleScript>().isTurn = false;
                Debug.Log("turn done");
                
            }
            if (players[GlobalController.turn].GetComponent<MeleePlayerBattleController>() != null)
            {
                players[GlobalController.turn].GetComponent<MeleePlayerBattleController>().isTurn = true;

            }
            else
            {

                players[GlobalController.turn].GetComponent<EnemyBattleScript>().isTurn = true;
            }
            
            turn = GlobalController.turn;
        }
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
