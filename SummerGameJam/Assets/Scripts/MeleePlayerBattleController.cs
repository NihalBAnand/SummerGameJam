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
    public bool updateCurrent = false;

    public Collider2D colidepos;

    public bool enemyAdj = false;
    public bool playerAdj = false;

    public string type = "melee";

    public Text moveText;
    public Text alert;
    public Text hpText;

    public List<GameObject> collidingObj = new List<GameObject>();
    public List<Vector3> currentCollisions = new List<Vector3>();
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
        GlobalController.turn += 1;
        currentCollisions.Clear();
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
        Debug.Log("col");
        if (!currentCollisions.Contains(collision.attachedRigidbody.transform.position))
        {
            currentCollisions.Add(collision.attachedRigidbody.transform.position);
        }
        if (!collidingObj.Contains(collision.gameObject))
        {
            collidingObj.Add(collision.gameObject);
        }
        inZone = true;
        colidepos = collision;
        checkAllcol();


    }
    String col(Collider2D collider)
    {
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
    /*    if (other.gameObject.GetComponent<MeleePlayerBattleController>() != null)
        {
            other.gameObject.GetComponent<MeleePlayerBattleController>().currentCollisions.Clear();
        }
        else
        {
            other.gameObject.GetComponent<EnemyBattleScript>().currentCollisions.Clear();
        }*/
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
         
        if (!currentCollisions.Contains(colidepos.attachedRigidbody.transform.position)){
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
