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
    public List<Vector3> currentCollisions = new List<Vector3>();
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
            endTurn();
        }
        if (isTurn)
        {
            if (!selected)
            {
                Debug.Log("Selecting");
                selectTarget();
            }
            if ((moved < speed || !atEnemy) && inZone == false)
            {
                Move();

            }
            else if (moved < speed && inZone == true)
            {
                checkAllcol();
                Move();
                checkAllcol();
            }
        }
        
     

    }
    void endTurn()
    {
        GlobalController.turn += 1;
        isTurn = false;
        atEnemy = false;
        moved = 0;
        selected = false;
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
                
                }

                //down
            }
        }
        xy*=-1;
        Thread.Sleep(200);
    }





    void selectTarget()
    {
        battleStart = GameObject.Find("BattleStart").GetComponent<BattleStart>();
        
        float dist = Vector3.Distance(battleStart.players[0].transform.position, gameObject.transform.position);
        player = battleStart.players[0].transform.position;
        float mindist =  dist;
        for (int i = 1; i< battleStart.players.Count; i++)
        {
            if(battleStart.players[i].GetComponent<MeleePlayerBattleController>() != null)
            {
                dist = Vector3.Distance(battleStart.players[i].transform.position, gameObject.transform.position);
                if (mindist > dist)
                {
                    player = battleStart.players[i].transform.position;
                    mindist = dist;
                }
            }
        }
        selected = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        currentCollisions.Add(collision.attachedRigidbody.transform.position);
        inZone = true;
        colidepos = collision;
        checkAllcol();


    }
    String col(Collider2D collider)
    {
        UnityEngine.Vector3 otherpos = collider.attachedRigidbody.transform.position;
        UnityEngine.Vector3 myPos = gameObject.transform.position;
        UnityEngine.Vector3 checkPos;
        if (isTurn)
        {
            checkPos = new Vector3(myPos.x - 1, myPos.y);
            if (currentCollisions.Contains(checkPos))
            {
                if (!enemyDirections.Contains("L"))
                    return "L";

            }
            else if (enemyDirections.Contains("L"))
            {
                enemyDirections.Remove("L");
            }
            checkPos = new Vector3(myPos.x + 1, myPos.y);
            if (currentCollisions.Contains(checkPos))
            {
                if (!enemyDirections.Contains("R"))
                    return "R";

            }
            else if (enemyDirections.Contains("R"))
            {
                enemyDirections.Remove("R");
            }
            checkPos = new Vector3(myPos.x , myPos.y -1);
            if (currentCollisions.Contains(checkPos))
            {
                if (!enemyDirections.Contains("D"))
                    return "D";
            }
            else if (enemyDirections.Contains("D"))
            {
                enemyDirections.Remove("D");
            }
            checkPos = new Vector3(myPos.x , myPos.y+1);
            if (currentCollisions.Contains(checkPos))
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
        Vector3 otherVec = other.attachedRigidbody.transform.position;
        if (currentCollisions.Contains(otherVec))
        {
            currentCollisions.Remove(otherVec);
        }
        if (currentCollisions.Count == 0)
        {
            enemyDirections.Clear();
            inZone = false;
        }
        if (!isTurn)
        {
            currentCollisions.Clear();
        }
    }
    void checkAllcol()
    {
        currentCollisions.Add(colidepos.attachedRigidbody.transform.position);
        foreach (Vector3 c in currentCollisions)
        {
            enemyDirections.Add(col(colidepos));
            enemyDirections.Remove("none");
        }
    }
}

