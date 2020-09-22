using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class TurnBasedBattleManager : MonoBehaviour
{
    public List<GameObject> players;
    public List<GameObject> enemies;
    public List<GameObject> battlers;

    public bool partyTurn;

    public int totalPartySpeed;
    public int totalEnemySpeed;

    public static int currentTurn;
    public int debugTurn;
    // Start is called before the first frame update
    void Start()
    {
        //initialize lists
        players = new List<GameObject>();
        enemies = new List<GameObject>();
        battlers = new List<GameObject>();

        //add party members to players list and battlers list
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("player"))
        {
            //Debug.Log(g);
            players.Add(g);
            battlers.Add(g);
        }

        //add enemies to enemies list and battlers list
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("enemy"))
        {
            //Debug.Log(g);
            enemies.Add(g);
            battlers.Add(g);
        }

        //Determine turn order
        battlers.Sort((x, y) => x.GetComponent<TurnBasedBattler>().speed.CompareTo(y.GetComponent<TurnBasedBattler>().speed));
        battlers.Reverse();
        currentTurn = 0;

    }

    // Update is called once per frame
    void Update()
    {
        debugTurn = currentTurn;
        if (currentTurn >= battlers.Count)
        {
            currentTurn = 0;
        }
        battlers[currentTurn].GetComponent<TurnBasedBattler>().Turn();
    }
}
