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
    public int enemies;
    public int ppl;
    int amount = 0;
    int xpos;
    int ypos;
    int turn;
    public List<GameObject> players = new List<GameObject>();
  
    void Start()
    {
        enemies = GlobalController.enemies;
        ppl = GlobalController.partymembers;
        turn = GlobalController.turn;
        Debug.Log(ppl);
        SpawnParty();
        SpawnEnemies();
        scramble();
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
            ypos = Random.Range(-5, 10);
            players.Add(Instantiate(enemie, new Vector3(xpos, ypos), Quaternion.identity));
            amount++;

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
            players.Add(Instantiate(partyMember, new Vector3(xpos, ypos), Quaternion.identity));
            amount++;

        }
    }
    void handleTurn()
    {
        if (GlobalController.turn == players.Count)
        {
            GlobalController.turn = 0;

        }
        else if (GlobalController.turn > turn || GlobalController.turn < turn)
        {
            Debug.Log(GlobalController.turn);
            try
            {
                players[turn].GetComponent<MeleePlayerBattleController>().isTurn = false;
            }
            catch (Exception)
            {
                players[turn].GetComponent<EnemyBattleScript>().isTurn = false;
                
            }
            try
            {
                players[GlobalController.turn].GetComponent<MeleePlayerBattleController>().isTurn = true;

            }
            catch (Exception)
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
