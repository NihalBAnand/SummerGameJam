using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class MeleePlayerBattleController : MonoBehaviour
{
    public int speed = 6;
    public int moved = 0;
    public int dmg   = 10;
    public int hp    = 100;
    public bool isTurn = false;
    public bool isEnemyTurn = false;
    public bool attacked = false;
    public bool inZone = true;

    public Collider2D colidepos;

    public bool enemyAdj = false;
    public bool playerAdj = false;

    public string type = "melee";

    public Text moveText;
    public Text alert;
    public Text hpText;

    public List<string> enemyDirections = new List<string>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> adjEnemies = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> adjPlayers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
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
        //DetectObjects();
        if (isTurn)
        {
            if (moved < speed && inZone == false )
                Movement();
            else if(moved < speed)
            {
                Movement();
                enemyDirections.Add(col(colidepos));
                enemyDirections.Remove("none");
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
        GlobalController.turn += 1;
    }


    /*    IEnumerator EndTurn()
        {
            isTurn = false;
            isEnemyTurn = true;
            yield return new WaitUntil(() => !isEnemyTurn);
            moved = 0;
            attacked = false;
            isTurn = true;
        }*/

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!enemyDirections.Contains("L"))
            {
                gameObject.transform.position = new UnityEngine.Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!enemyDirections.Contains("R"))
            {
                gameObject.transform.position = new UnityEngine.Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!enemyDirections.Contains("U"))
            {
                gameObject.transform.position = new UnityEngine.Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!enemyDirections.Contains("D"))
            {
                gameObject.transform.position = new UnityEngine.Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                moved++;
            }
        }
    }

    void UpdateUI()
    {
        moveText.text = "Movement left: " + (speed - moved);
        alert.enabled = enemyAdj;
        hpText.text = "HP: " + hp;
    }


    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enemyAdj)
            {
                foreach (GameObject e in adjEnemies)
                {
                    e.GetComponent<EnemyBattleScript>().health -= dmg;
                    attacked = true;
                    Debug.Log(e.GetComponent<EnemyBattleScript>().health);
                }
                
            }
        }
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
        inZone= false;
    }

}
