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
    public MeleePlayerBattleController player;
    public MeleePlayerBattleController Activeplayer;
    public BattleStart battleStart;


    // Start is called before the first frame update d
    void Start()
    {
        battleStart = GameObject.Find("BattleStart").GetComponent<BattleStart>();
        Activeplayer = battleStart.players[0].GetComponent<MeleePlayerBattleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (Activeplayer.isEnemyTurn)
        {
            moved = 0;
            StartCoroutine(Move());
            Activeplayer.isEnemyTurn = false;

        }
        
    }

    IEnumerator Move()
    {
        player = battleStart.players[Random.Range(0, battleStart.players.Count)].GetComponent<MeleePlayerBattleController>();
        while (!player.adjEnemies.Contains(gameObject) && moved < speed)
        {
            Debug.Log("TEST");
            if (gameObject.transform.position.x < player.gameObject.transform.position.x)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                moved++;
                yield return new WaitForSeconds(.5f);
                Debug.Log("RIGHT!");
            }
            else if (gameObject.transform.position.x > player.gameObject.transform.position.x)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                moved++;
                yield return new WaitForSeconds(.5f);
            }
            else if (gameObject.transform.position.y < player.gameObject.transform.position.y)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                moved++;
                yield return new WaitForSeconds(.5f);
                Debug.Log("UP!");
            }
            else if (gameObject.transform.position.y > player.gameObject.transform.position.y)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                moved++;
                yield return new WaitForSeconds(.5f);
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
        

    }
}
