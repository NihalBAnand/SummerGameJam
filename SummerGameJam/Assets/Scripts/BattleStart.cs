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
    public List<GameObject> players = new List<GameObject>();
    void Start()
    {
        ppl = GlobalController.partymembers;
        Debug.Log(ppl);
        while (amount < ppl)
        {
            xpos = Random.Range(1, 10);
            ypos = Random.Range(1, 10);
            players.Add(Instantiate(partyMember, new Vector3(xpos, ypos), Quaternion.identity));
            amount++;


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
