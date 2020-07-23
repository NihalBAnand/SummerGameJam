using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleStart : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject partyMember;
    public int ppl;
    int amount = 0;
    int xpos;
    int ypos;
    int turn;
    public List<GameObject> players = new List<GameObject>();
  
    void Start()
    {
        ppl = GlobalController.partymembers;
        turn = GlobalController.turn;
        Debug.Log(ppl);
        while (amount < ppl)
        {
            xpos = Random.Range(1, 10);
            ypos = Random.Range(1, 10);
            players.Add(Instantiate(partyMember, new Vector3(xpos, ypos), Quaternion.identity));
            amount++;

           // players[amount].GetComponent<MeleePlayerBattleController>();

        }
        for (int i = 0; i < players.Count; i++)
        {
            GameObject temp = players[i];
            int randomIndex = Random.Range(i, players.Count);
            players[i] = players[randomIndex];
            players[randomIndex] = temp;
        }
        players[0].GetComponent<MeleePlayerBattleController>().isTurn = true;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (GlobalController.turn==players.Count)
        {
            GlobalController.turn = 0;

        }
        else if (GlobalController.turn > turn || GlobalController.turn < turn) 
        {
            Debug.Log(GlobalController.turn);
            players[turn].GetComponent<MeleePlayerBattleController>().isTurn = false;
            players[GlobalController.turn].GetComponent<MeleePlayerBattleController>().isTurn = true;
            turn = GlobalController.turn;
        }
    }
}
