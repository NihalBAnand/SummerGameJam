using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Threading;
using System.Linq.Expressions;
using TMPro;

public class EnemyBattleScript : MonoBehaviour
{
    public int health = 50;
    public int dmg = 10;
    public int speed = 0;
    public int moved = 0;
    public bool isTurn = false;
    public Vector3 player;
    public MeleePlayerBattleController Activeplayer;
    public BattleStart battleStart;
    private GameObject temp;
    public bool inZone = false;
    public List<string> enemyDirections = new List<string>();
    public Collider2D colidepos;
    public bool adjenemy = false;
    public bool atEnemy = false;
    public bool moving = false;
    public bool selected = false;
    int turn;
    public int xy = -1;


    // Start is called before the first frame update d
    void Start()
    {
        
        //battleStart = GameObject.Find("BattleStart").GetComponent<BattleStart>();
        //Activeplayer = battleStart.players[0].GetComponent<MeleePlayerBattleController>();
        speed = 100;
        


    }



    // Update is called once per frame
    void Update()
    {
        /* if (health <= 0)
         {
             Destroy(gameObject);
         }*/

        if (moved >= speed || atEnemy == true)
        {
            Debug.Log("fasdfsdfdafsad");
            endTurn();
        }
        if (isTurn)
        {
            if (!selected)
            {
                selectTarget();
            }
            if ((moved < speed || !atEnemy) && inZone == false)
            {
                Move();

            }
            else if (moved < speed || !atEnemy)
            {
                Move();
                enemyDirections.Add(col(colidepos));
                enemyDirections.Remove("none");
            }
        }
        
     

    }
    void endTurn()
    {
       /* GlobalController.turn += 1;
        isTurn = false;
        atEnemy = false;
        moved = 0;
        selected = false;*/
    }
    void Move()
    {
        
        if (xy > 0)
        {
            if (player.x > gameObject.transform.position.x)
            {
                if (enemyDirections.Contains("R") && gameObject.transform.position.y == player.y && gameObject.transform.position.x + 1 == player.x)
                {
                    atEnemy = true;
                }
                else if (!enemyDirections.Contains("R"))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                    moved++;
                    Debug.Log("R");
                }

                //Right
            }
            else if (player.x < gameObject.transform.position.x)
            {
                if (enemyDirections.Contains("L") && gameObject.transform.position.y == player.y && gameObject.transform.position.x - 1 == player.x)
                {
                    atEnemy = true;
                }
                else if (!enemyDirections.Contains("L"))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                    moved++;
                    Debug.Log("L");
                }

                //Left
            }
        }
        else
        {
            if (player.y > gameObject.transform.position.y)
            {
                if (enemyDirections.Contains("U") && gameObject.transform.position.x == player.x && gameObject.transform.position.y + 1 == player.y)
                {
                    atEnemy = true;
                }
                else if (!enemyDirections.Contains("U"))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                    moved++;
                    Debug.Log("U");
                }
                //up
            }
            else if (player.y < gameObject.transform.position.y)
            {
                if (enemyDirections.Contains("D") && gameObject.transform.position.x == player.x && gameObject.transform.position.y - 1 == player.y)
                {
                    atEnemy = true;
                }
                else if (!enemyDirections.Contains("D"))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                    moved++;
                    Debug.Log("D");
                }

                //down
            }
        }
        xy*=-1;
        Thread.Sleep(200);
    }


    /*        while (true)
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
                yield return new WaitForSeconds(.5f);
                //yield return new WaitForEndOfFrame();
                if (gameObject.transform.position.x < player.gameObject.transform.position.x && !enemyDirections.Contains("R"))
                {

                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y  );
                    moved++;

                    Debug.Log("RIGHT!");
                }
                else if (gameObject.transform.position.x > player.gameObject.transform.position.x && !enemyDirections.Contains("L"))
                {

                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                    moved++;

                }
                else if (gameObject.transform.position.y < player.gameObject.transform.position.y && !enemyDirections.Contains("U"))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                    moved++;

                    Debug.Log("UP!");
                }
                else if (gameObject.transform.position.y > player.gameObject.transform.position.y && !enemyDirections.Contains("D"))
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

*/


    void selectTarget()
    {
        battleStart = GameObject.Find("BattleStart").GetComponent<BattleStart>();
        float maxdist = 0;
        for(int i = 0; i< battleStart.players.Count; i++)
        {
            if(battleStart.players[i].GetComponent<MeleePlayerBattleController>() != null)
            {
                float dist = Vector3.Distance(battleStart.players[i].transform.position, gameObject.transform.position);
                Debug.Log(dist);
                if (maxdist < dist)
                {
                    Debug.Log("kjhdfsahjklfdsahklj");
                    player = battleStart.players[i].transform.position;
                    Debug.Log(player);
                    maxdist = dist;
                }
            }
        }
        selected = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        inZone = true;
        colidepos = collision;
        enemyDirections.Add(col(collision));
        enemyDirections.Remove("none");


    }
    String col(Collider2D collider)
    {
        UnityEngine.Vector3 otherpos = collider.attachedRigidbody.transform.position;
        UnityEngine.Vector3 myPos = gameObject.transform.position;
        if (isTurn)
        {

            if (myPos.x > otherpos.x && myPos.y == otherpos.y)
            {
                if (!enemyDirections.Contains("L"))
                    return "L";

            }
            else if (enemyDirections.Contains("L"))
            {
                enemyDirections.Remove("L");
            }
            if (myPos.x < otherpos.x && myPos.y == otherpos.y)
            {
                if (!enemyDirections.Contains("R"))
                    return "R";

            }
            else if (enemyDirections.Contains("R"))
            {
                enemyDirections.Remove("R");
            }
            if (myPos.y > otherpos.y && myPos.x == otherpos.x)
            {
                if (!enemyDirections.Contains("D"))
                    return "D";
            }
            else if (enemyDirections.Contains("D"))
            {
                enemyDirections.Remove("D");
            }

            if (myPos.y < otherpos.y && myPos.x == otherpos.x)
            {
                if (!enemyDirections.Contains("U"))
                    return "U";
            }
            else if (enemyDirections.Contains("U"))
            {
                enemyDirections.Remove("U");
            }
        }
        return "none";

    }
    void OnTriggerExit2D(Collider2D other)
    {
        enemyDirections.Clear();
        inZone = false;
    }
    
}

