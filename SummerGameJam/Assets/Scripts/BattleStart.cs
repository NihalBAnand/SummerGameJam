using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart : MonoBehaviour
{
    // Start is called before the first frame update
    int melee = 0;
    int ranged = 0;
    int enemy = 0;
    void Start()
    {
        for(int i = 0; i<players.Length; i++)
        {
            Debug.Log(players[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
