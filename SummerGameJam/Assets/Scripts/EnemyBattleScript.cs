using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBattleScript : MonoBehaviour
{
    public int health = 50;
    public int dmg    = 10;
    public int speed  = 4;
    public int moved  = 0;
    public bool isTurn = false;
    public MeleePlayerBattleController player;
    public MeleePlayerBattleController Activeplayer;
    public BattleStart battleStart;
    private GameObject temp;


    // Start is called before the first frame update d
    void Start()
    {
        battleStart = GameObject.Find("BattleStart").GetComponent<BattleStart>();
        Activeplayer = battleStart.players[0].GetComponent<MeleePlayerBattleController>();
        speed = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (isTurn)
        {
            StartCoroutine(Move());
            

        }
        
    }

    IEnumerator Move()
    {
        
        while (true)
        {
            player = battleStart.players[Random.Range(0, battleStart.players.Count - 1)].GetComponent<MeleePlayerBattleController>();
            if (player != null)
            {
                break;
            }
        }
        Debug.Log(player);
        while ((!player.adjEnemies.Contains(gameObject)) && moved < speed)
        {
            Debug.Log("TEST");
            //yield return new WaitForSeconds(.5f);
            yield return new WaitForEndOfFrame();
            if (gameObject.transform.position.x < player.gameObject.transform.position.x)
            {
                
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                moved++;
                
                Debug.Log("RIGHT!");
            }
            else if (gameObject.transform.position.x > player.gameObject.transform.position.x)
            {
                
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                moved++;
                
            }
            else if (gameObject.transform.position.y < player.gameObject.transform.position.y)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                moved++;
                
                Debug.Log("UP!");
            }
            else if (gameObject.transform.position.y > player.gameObject.transform.position.y)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                moved++;
                
            }
            else
            {
                break;
            }

        }
        
        if (player.adjEnemies.Contains(gameObject))
        {
            player.hp -= dmg;
        }
        
        GlobalController.turn += 1;
        moved = 0;

    }
}
