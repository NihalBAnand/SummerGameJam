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
    public BattleManager battleManager;
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
    public int[] dir = new int[4];
    public String lastDir = "";
    public String[] tempStr = { "R", "L", "U", "D" };



    // Start is called before the first frame update d
    void Start()
    {
        
        //BattleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        //Activeplayer = battleStart.players[0].GetComponent<MeleePlayerBattleController>();
        speed = 100;

        lastDir = "";

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
        for (int i = 0; i < dir.Length; i++)
        {
            dir[i] = 0;
        }
    }
    void Move()
    {

        /*if (xy > 0)
        {
            if (player.x > gameObject.transform.position.x)
            {
                if (enemyDirections.Contains("R") && gameObject.transform.position.y == player.y && gameObject.transform.position.x + 1 == player.x)
                {
                    atEnemy = true;
                }
                else if (!enemyDirections.Contains("R"))
                {
                    Vector3 prev = gameObject.transform.position;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                    moved++;
                    Vector3 now = gameObject.transform.position;
                    Vector3[] transition = new Vector3[] { prev, now };
                    BroadcastAll("updateColl", transition);

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
                    Vector3 prev = gameObject.transform.position;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                    moved++;
                    Vector3 now = gameObject.transform.position;
                    Vector3[] transition = new Vector3[] { prev, now };
                    BroadcastAll("updateColl", transition);

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
                    Vector3 prev = gameObject.transform.position;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                    moved++;
                    Vector3 now = gameObject.transform.position;
                    Vector3[] transition = new Vector3[] { prev, now };
                    BroadcastAll("updateColl", transition);
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
                    Vector3 prev = gameObject.transform.position;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                    moved++;
                    Vector3 now = gameObject.transform.position;
                    Vector3[] transition = new Vector3[] { prev, now };
                    BroadcastAll("updateColl", transition);


                }

                //down
            }
        }*/
        movePos(moveDir());
        xy*=-1;
        BoxCollider2D box = GetComponent<BoxCollider2D>(); 
        box.size = new Vector3(box.size.x -.1f, box.size.y - .1f);
        box.size = new Vector3(box.size.x +.1f, box.size.y + .1f);
        Thread.Sleep(200);
    }





    void selectTarget()
    {
        battleManager = GameObject.Find("BattleStart").GetComponent<BattleManager>();
        
        float dist = Vector3.Distance(battleManager.players[0].transform.position, gameObject.transform.position);
        player = battleManager.players[0].transform.position;
        float mindist =  dist;
        for (int i = 1; i< battleManager.players.Count; i++)
        {
            if(battleManager.players[i].GetComponent<MeleePlayerBattleController>() != null)
            {
                dist = Vector3.Distance(battleManager.players[i].transform.position, gameObject.transform.position);
                if (mindist > dist)
                {
                    player = battleManager.players[i].transform.position;
                    mindist = dist;
                }
            }
        }
        selected = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (!currentCollisions.Contains(collision.attachedRigidbody.transform.position))
        {
            currentCollisions.Add(collision.attachedRigidbody.transform.position);
        }
        inZone = true;
        colidepos = collision;
        checkAllcol();


    }
    String col(Collider2D collider)
    {
        UnityEngine.Vector3 otherpos = collider.attachedRigidbody.transform.position;
        UnityEngine.Vector3 myPos = gameObject.transform.position;
        UnityEngine.Vector3 checkPos;

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
        checkPos = new Vector3(myPos.x, myPos.y - 1);
        if (currentCollisions.Contains(checkPos))
        {
            if (!enemyDirections.Contains("D"))
                return "D";
        }
        else if (enemyDirections.Contains("D"))
        {
            enemyDirections.Remove("D");
        }
        checkPos = new Vector3(myPos.x, myPos.y + 1);
        if (currentCollisions.Contains(checkPos))
        {
            if (!enemyDirections.Contains("U"))
                return "U";
        }
        else if (enemyDirections.Contains("U"))
        {
            enemyDirections.Remove("U");
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
            //currentCollisions.Clear();
        }
    }
    void checkAllcol()
    {
        if (!currentCollisions.Contains(colidepos.attachedRigidbody.transform.position))
        {
            currentCollisions.Add(colidepos.attachedRigidbody.transform.position);
        }
        foreach (Vector3 c in currentCollisions)
        {
            enemyDirections.Add(col(colidepos));
            enemyDirections.Remove("none");
        }
    }
    public void BroadcastAll(string fun, System.Object msg)
    {
        GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in gos)
        {

            if (go != gameObject && (go && go.transform.parent == null))
            {
                go.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void updateColl(Vector3[] vs)
    {
        if (currentCollisions.Contains(vs[0]))
        {
            currentCollisions.Remove(vs[0]);
            currentCollisions.Add(vs[1]);
        }
    }
    public String moveDir()
    {
        selectTarget();
        targetDetect();
        if (!atEnemy)
        {

            //0 = R, 1 = L, 2 = U, 4 = D               
            if (enemyDirections.Contains("R"))
            {
                dir[0] = -1000;
            }
            else if (enemyDirections.Contains("L"))
            {
                dir[1] = -1000;
            }
            else if (enemyDirections.Contains("U"))
            {
                dir[2] = -1000;
            }
            else if (enemyDirections.Contains("D"))
            {
                dir[3] = -1000;
            }
            else
            {
                for (int i = 0; i < dir.Length; i++)
                {
                    dir[i] = 0;
                }
            }

            switch (lastDir)
            {
                case "R":
                    dir[0] -= 1;
                    break;
                case "L":
                    dir[1] -= 1;
                    break;
                case "U":
                    dir[2] -= 1;
                    break;
                case "D":
                    dir[3] -= 1;
                    break;
                default:
                    break;

            }

            /*
            if (xy > 0)
            {

                if (player.x > gameObject.transform.position.x)
                {
                    dir[0] += 2;
                }
                else if (player.x < gameObject.transform.position.x)
                {
                    dir[1] += 2;
                }
                else
                {
                    return "";
                }

            }
            
            if (xy < 0 && player.x == gameObject.transform.position.x)
            {
                if (player.y > gameObject.transform.position.y)
                {
                    dir[2] += 2;
                }
                else if (player.y < gameObject.transform.position.y)
                {
                    dir[3] += 2;
                }
                else
                {
                    return "";
                }
            }
            */

            if (player.x > gameObject.transform.position.x)
            {
                dir[0] += 3;
            }
            if (player.x < gameObject.transform.position.x)
            {
                dir[1] += 3;
            }
            if (player.y > gameObject.transform.position.y)
            {
                dir[2] += 2;
            }
            if (player.y < gameObject.transform.position.y)
            {
                dir[3] += 2;
            }

            if (lastDir == "L" && player.y > gameObject.transform.position.y)
            {
                dir[2] += 3;
            }
            else if (lastDir == "L" && player.y < gameObject.transform.position.y)
            {
                dir[3] += 3;
            }
            if (lastDir == "R" && player.y > gameObject.transform.position.y)
            {
                dir[2] += 3;
            }
            else if (lastDir == "R" && player.y < gameObject.transform.position.y)
            {
                dir[3] += 3;
            }




            String direction = "";
            int maxDirVal = 0;
            for (int i = 0; i < dir.Length; i++)
            {
                if (dir[i] >= maxDirVal)
                {
                    direction = tempStr[i];
                    maxDirVal = dir[i];
                }

            }
            Debug.Log(direction);
            lastDir = direction;
            return direction;
            
        }
        return "";


    }
    public void movePos(string s)
    {
        if(s == "R")
        {
            Vector3 prev = gameObject.transform.position;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
            moved++;
            Vector3 now = gameObject.transform.position;
            Vector3[] transition = new Vector3[] { prev, now };
            BroadcastAll("updateColl", transition);
        }
        else if(s == "L")
        {
            Vector3 prev = gameObject.transform.position;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
            moved++;
            Vector3 now = gameObject.transform.position;
            Vector3[] transition = new Vector3[] { prev, now };
            BroadcastAll("updateColl", transition);
        }
        else if(s == "U")
        {
            Vector3 prev = gameObject.transform.position;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
            moved++;
            Vector3 now = gameObject.transform.position;
            Vector3[] transition = new Vector3[] { prev, now };
            BroadcastAll("updateColl", transition);
        }
        else if(s == "D")
        {
            Vector3 prev = gameObject.transform.position;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
            moved++;
            Vector3 now = gameObject.transform.position;
            Vector3[] transition = new Vector3[] { prev, now };
            BroadcastAll("updateColl", transition);
        }


    }
    public void targetDetect()
    {
        if ((enemyDirections.Contains("R") && gameObject.transform.position.y == player.y && gameObject.transform.position.x + 1 == player.x)
          || (enemyDirections.Contains("L") && gameObject.transform.position.y == player.y && gameObject.transform.position.x - 1 == player.x)
          || (enemyDirections.Contains("U") && gameObject.transform.position.x == player.x && gameObject.transform.position.y + 1 == player.y)
          || (enemyDirections.Contains("D") && gameObject.transform.position.x == player.x && gameObject.transform.position.y - 1 == player.y))
        {
            atEnemy = true;
        }
    }
}

