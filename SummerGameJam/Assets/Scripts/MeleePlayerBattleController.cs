using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;
using Random = UnityEngine.Random;


public class MeleePlayerBattleController : MonoBehaviour
{
    public int speed = 6;
    public int moved = 0;
    public int dmg   = 10;
    public int hp    = 100;
    public int maxHp = 50;
    public float range = 7; 
    public bool isTurn = false;
    public bool isEnemyTurn = false;
    public bool attacked = false;
    public bool inZone = true;
    public bool updateCurrent = false;


    public Collider2D colidepos;

    public bool healer = false;
    public bool ranged = false;
    public bool enemyAdj = false;
    public bool playerAdj = false;

    public string type = "melee";

    public Text moveText;
    public Text alert;
    public Text hpText;

    public List<GameObject> adjEnemies = new List<GameObject>();
    public List<Vector3> currentCollisions = new List<Vector3>();
    public List<string> enemyDirections = new List<string>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> adjPlayers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        speed = 7;
        hp = 50;
        maxHp = 50;
        inZone = false;
        moveText = GameObject.FindGameObjectWithTag("Moves").GetComponent<Text>();
        alert = GameObject.FindGameObjectWithTag("alert").GetComponent<Text>();
        hpText = GameObject.FindGameObjectWithTag("hp").GetComponent<Text>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("enemy"))
        {
            enemies.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("player"))
        {
            players.Add(obj);
        }
        alert.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        isTurn = gameObject.GetComponent<Turn>().isTurn;
        checkHealth();
        //DetectObjects();
        if (isTurn)
        {
          /*if(GlobalController.turnCycle == 0 && GlobalController.turn == 0)
            {
                
                if (Random.Range(1,players.Count) == 1)
                {
                    EndTurn();
                }
            }*/
            if (moved < speed && inZone == false )
                Movement();
            else if(moved < speed)
            {
                checkAllcol();
                Movement();
                checkAllcol();
            }
            
            if (!attacked)
                Attack();
            UpdateUI();
            if (Input.GetKeyDown(KeyCode.Return))
                EndTurn();

        }
        
    }

    public void EndTurn()
    {
        BattleManager battleManager = GameObject.Find("BattleStart").GetComponent<BattleManager>();
        GlobalController.turn += 1;
        if(GlobalController.turn>=battleManager.players.Count) 
        {
            GlobalController.turn = 0;
        }
        battleManager.players[GlobalController.turn].GetComponent<Turn>().isTurn = true;
        gameObject.GetComponent<Turn>().isTurn = false;
        attacked = false;
        isTurn = false;
        moved = 0;
        //currentCollisions.Clear();
    }



    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!enemyDirections.Contains("L"))
            {
                Vector3 prev = gameObject.transform.position;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                moved++;
                Vector3 now = gameObject.transform.position;
                Vector3[] transition = new Vector3[] { prev, now };
                BroadcastAll("updateColl", transition);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!enemyDirections.Contains("R"))
            {
                Vector3 prev = gameObject.transform.position;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                moved++;
                Vector3 now = gameObject.transform.position;
                Vector3[] transition = new Vector3[] { prev, now };
                BroadcastAll("updateColl", transition);
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!enemyDirections.Contains("U"))
            {
                Vector3 prev = gameObject.transform.position;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                moved++;
                Vector3 now = gameObject.transform.position;
                Vector3[] transition = new Vector3[] { prev, now };
                BroadcastAll("updateColl", transition);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!enemyDirections.Contains("D"))
            {
                Vector3 prev = gameObject.transform.position;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                moved++;
                Vector3 now = gameObject.transform.position;
                Vector3[] transition = new Vector3[] { prev, now };
                BroadcastAll("updateColl", transition);
            }
        }
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = new Vector3(box.size.x - .1f, box.size.y - .1f);
        box.size = new Vector3(box.size.x + .1f, box.size.y + .1f);
    }

    void UpdateUI()
    {
        moveText.text = "Movement left: " + (speed - moved);
        alert.enabled = enemyAdj;
        hpText.text = "HP: " + hp;
    }
    void checkHealth()
    {
        if (hp <= 0)
        {
            BattleManager battleManager = GameObject.Find("BattleStart").GetComponent<BattleManager>();
            if (isTurn)
            {
                GlobalController.turn += 1;
                if (GlobalController.turn >= battleManager.players.Count)
                {
                    GlobalController.turn = 0;
                }

                battleManager.players[GlobalController.turn].GetComponent<Turn>().isTurn = true;
            }
            battleManager.players.Remove(gameObject);
            battleManager.ppl -= 1;
            GameObject.Destroy(gameObject);
        }
    }

    void Attack()
    {
        BattleManager battleManager = GameObject.Find("BattleStart").GetComponent<BattleManager>();
        int count = battleManager.players.Count;
        Vector3 ePos;
        if (ranged == false && healer ==  false)
        {
            if (adjEnemies.Count > 0)
            {
                enemyAdj = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (enemyAdj)
                {
                    foreach (GameObject e in adjEnemies)
                    {
                        ePos = e.transform.position;
                        if(e.GetComponent<EnemyBattleScript>().health - dmg == 0)
                        {
                            currentCollisions.Remove(ePos);
                        }
                        e.GetComponent<EnemyBattleScript>().health -= dmg;
                        attacked = true;
                        Debug.Log(e.GetComponent<EnemyBattleScript>().health);
                    }

                }
            }
        }
        else if(ranged == true && healer == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                GameObject target = battleManager.players[0];
                float minDist = 1000000000000000;
                foreach (GameObject g in battleManager.players)
                {

                    if (g.GetComponent<EnemyBattleScript>() != null && Vector3.Distance(g.transform.position, gameObject.transform.position) < minDist)
                    {
                        target = g;
                        minDist = Vector3.Distance(g.transform.position, gameObject.transform.position);
                    }
                }
                if (minDist <= range)
                {
                    target.GetComponent<EnemyBattleScript>().health -= dmg;
                    attacked = true;
                }
            }
        }
       
        if (attacked == true && count != battleManager.players.Count)
        {
            Debug.Log("yeet");
            GlobalController.turn = battleManager.players.Count -1;
        }
        if(ranged == false && healer == true)
        {
            if (adjPlayers.Count > 0)
            {
                playerAdj= true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (playerAdj)
                {
                    foreach (GameObject e in adjPlayers)
                    {   if(e.GetComponent<MeleePlayerBattleController>().hp + dmg< maxHp)
                            e.GetComponent<MeleePlayerBattleController>().hp += dmg;
                        else
                        {
                            e.GetComponent<MeleePlayerBattleController>().hp = maxHp;
                        }
                        attacked = true;
                        Debug.Log(e.GetComponent<MeleePlayerBattleController>().hp);
                    }

                }
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("col");
        if (!currentCollisions.Contains(collision.attachedRigidbody.transform.position))
        {
            currentCollisions.Add(collision.attachedRigidbody.transform.position);
        }
        if (!adjEnemies.Contains(collision.gameObject))
        {
            if (collision.gameObject.GetComponent<EnemyBattleScript>() != null)
                adjEnemies.Add(collision.gameObject);
            else if (collision.gameObject.GetComponent<MeleePlayerBattleController>() != null) adjPlayers.Add(collision.gameObject); 
        }
     
        inZone = true;
        colidepos = collision;
        checkAllcol();


    }
    String col(Collider2D collider)
    {
        if(collider == null)
        {
            return "none";
        }
        Vector3 otherpos = collider.attachedRigidbody.transform.position;
        Vector3 myPos = gameObject.transform.position;
        Vector3 checkPos;

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
        if (adjEnemies.Contains(other.gameObject))
            adjEnemies.Remove(other.gameObject);
        if (adjPlayers.Contains(other.gameObject))
            adjPlayers.Remove(other.gameObject);

        if (!isTurn)
        {
            //currentCollisions.Clear();
        }
    }
    void checkAllcol()
    {
       
        if (colidepos != null && !currentCollisions.Contains(colidepos.attachedRigidbody.transform.position)){
            currentCollisions.Add(colidepos.attachedRigidbody.transform.position); }

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


}
