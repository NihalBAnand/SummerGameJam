using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyBattleScript : MonoBehaviour
{
    public int health = 50;
    public int dmg    = 10;
    public int speed  = 4;
    public int moved  = 0;
    public MeleePlayerBattleController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").GetComponent<MeleePlayerBattleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (player.isEnemyTurn)
        {
            moved = 0;
            StartCoroutine(Move());
            player.isEnemyTurn = false;

        }
        
    }

    IEnumerator Move()
    {
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
