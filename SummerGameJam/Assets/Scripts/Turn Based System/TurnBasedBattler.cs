using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedBattler : MonoBehaviour
{
    public int speed;
    public int health;
    public bool doneLoading = false;

    public string selected;
    public GameObject selectBox;
    public GameObject selectArrow;
    public GameObject attackText;
    public GameObject magicText;
    public GameObject itemText;
    public GameObject runText;
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
        attackText = GameObject.Find("Attack");
        magicText = GameObject.Find("Magic");
        itemText = GameObject.Find("Item");
        runText = GameObject.Find("Run");

        selectArrow.transform.position = new Vector3(attackText.transform.position.x - 100f, attackText.transform.position.y,0);
        selectBox.SetActive(false);
        selectArrow.SetActive(false);
        selected = "Attack";

    }

    public void Turn()
    {
        if(gameObject.tag == "player")
        {
            selectBox.SetActive(true);
            selectArrow.SetActive(true);
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selected == "Magic")
                {
                    selected = "Attack";
                    selectArrow.transform.position = new Vector3(attackText.transform.position.x - 100f, attackText.transform.position.y, 0);
                }
                if (selected == "Run")
                {
                    selected = "Item";
                    selectArrow.transform.position = new Vector3(itemText.transform.position.x - 100f, itemText.transform.position.y, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selected == "Attack")
                {
                    selected = "Magic";
                    selectArrow.transform.position = new Vector3(magicText.transform.position.x - 100f, magicText.transform.position.y, 0);
                }
                if (selected == "Item")
                {
                    selected = "Run";
                    selectArrow.transform.position = new Vector3(runText.transform.position.x - 100f, runText.transform.position.y, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selected == "Attack")
                {
                    selected = "Item";
                    selectArrow.transform.position = new Vector3(itemText.transform.position.x - 100f, itemText.transform.position.y, 0);
                }
                if (selected == "Magic")
                {
                    selected = "Run";
                    selectArrow.transform.position = new Vector3(runText.transform.position.x - 100f, runText.transform.position.y, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selected == "Item")
                {
                    selected = "Attack";
                    selectArrow.transform.position = new Vector3(attackText.transform.position.x - 100f, attackText.transform.position.y, 0);
                }
                if (selected == "Run")
                {
                    selected = "Magic";
                    selectArrow.transform.position = new Vector3(magicText.transform.position.x - 100f, magicText.transform.position.y, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                TurnBasedBattleManager.currentTurn += 1;
                selectBox.SetActive(true);
                selectArrow.SetActive(false);
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
