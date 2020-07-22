using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleStart : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject partyMember;
    int melee = 0;
    int ranged = 0;
    int enemy = 0;
    int ppl;
    int amount = 0;
    int xpos;
    int ypos;
    List<GameObject> players = new List<GameObject>();
    void Start()
    { 
        ppl = PlayerPrefs.GetInt("partymembers");
        Debug.Log(ppl);
        StartCoroutine(PlayerSpawn());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator PlayerSpawn()
    {
        
        while (amount < ppl)
        {
            xpos = Random.Range(1, 10);
            ypos = Random.Range(1, 10);
            players.Add(Instantiate(partyMember, new Vector3(xpos, ypos), Quaternion.identity));
            amount++;
            yield return new WaitForSeconds(0);

        }   

    }
}
