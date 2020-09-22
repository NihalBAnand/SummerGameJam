using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedBattler : MonoBehaviour
{
    public int speed;
    public int health;
    public bool doneLoading = false;

    public string[] battleOptions = { "Attack", "Item", "Magic", "Run" };
    public string selected;
    public GameObject selectBox;
    public GameObject selectArrow;
    // Start is called before the first frame update
    void Start()
    {
        speed = 10;
        if (gameObject.name == "Fighter")
        {
            speed = Battlers.fighter.speed;
            health = Battlers.fighter.health;
        }
        if (gameObject.name == "Zombie")
        {
            speed = Battlers.zombie.speed;
            health = Battlers.zombie.health;
        }
        if (gameObject.name == "Mage")
        {
            speed = Battlers.mage.speed;
            health = Battlers.mage.health;
        }
        doneLoading = true;

        selectBox = GameObject.FindGameObjectWithTag("selectbox");
        selectArrow = GameObject.Find("SelectArrow");
        selectBox.SetActive(false);
        selected = battleOptions[0];

    }

    public void Turn()
    {
        if(gameObject.tag == "player")
        {
            selectBox.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                
            }
        }
        if (gameObject.tag == "enemy")
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TurnBasedBattleManager.currentTurn += 1;
                GameObject.FindGameObjectsWithTag("player")[0].GetComponent<TurnBasedBattler>().health -= 10;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
