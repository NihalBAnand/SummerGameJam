using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TurnBasedBattler : MonoBehaviour
{
    public int speed;
    public int health;
    public int maxHealth;
    public bool doneLoading = false;

    public string selected;
    public GameObject selectBox;
    public GameObject selectArrow;
    public GameObject attackText;
    public GameObject magicText;
    public GameObject itemText;
    public GameObject runText;
    public Item activeItem;
    public Weapon activeWeapon;
    public Spell activeSpell;

    public Text turnDisp;
    // Start is called before the first frame update
    void Start()
    {
        speed = 10;
        if (gameObject.name == "Fighter")
        {
            speed = Battlers.fighter.speed;
            health = Battlers.fighter.health;
            maxHealth = Battlers.fighter.maxHealth;
            activeWeapon = Battlers.sword;
        }
        if (gameObject.name == "Zombie")
        {
            speed = Battlers.zombie.speed;
            health = Battlers.zombie.health;
            maxHealth = Battlers.zombie.maxHealth;
            activeWeapon = Battlers.fist;
        }
        if (gameObject.name == "Mage")
        {
            speed = Battlers.mage.speed;
            health = Battlers.mage.health;
            maxHealth = Battlers.mage.maxHealth;
            activeSpell = Battlers.weakFireball;
        }
        doneLoading = true;

        selectBox = GameObject.FindGameObjectWithTag("selectbox");
        selectArrow = GameObject.Find("SelectArrow");
        attackText = GameObject.Find("Attack");
        magicText = GameObject.Find("Magic");
        itemText = GameObject.Find("Item");
        runText = GameObject.Find("Run");
        turnDisp = GameObject.FindGameObjectWithTag("turndisp").GetComponent<Text>();

        selectArrow.transform.position = new Vector3(attackText.transform.position.x - 100f, attackText.transform.position.y,0);
        //selectBox.SetActive(false);
        //selectArrow.SetActive(false);
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
                /*selectBox.SetActive(false);
                selectArrow.SetActive(false);*/
                switch (selected)
                {
                    case ("Attack"):
                        GameObject.FindGameObjectWithTag("enemy").GetComponent<TurnBasedBattler>().health -= gameObject.GetComponent<TurnBasedBattler>().activeWeapon.damage;
                        break;
                    case ("Magic"):
                        gameObject.GetComponent<TurnBasedBattler>().activeSpell.Use(gameObject.GetComponent<TurnBasedBattler>(), GameObject.FindGameObjectWithTag("enemy").GetComponent<TurnBasedBattler>());
                        break;
                    case ("Item"):
                        gameObject.GetComponent<TurnBasedBattler>().activeItem.Use(gameObject.GetComponent<TurnBasedBattler>());
                        break;
                    case ("Run"):
                        Debug.Log("MASSIVE TO-DO: SWITCH BACK TO MAIN SCENE OF LEVEL");
                        break;
                }
            }
        }
        if (gameObject.tag == "enemy")
        {
            selectBox.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TurnBasedBattleManager.currentTurn += 1;
                GameObject.FindGameObjectsWithTag("player")[0].GetComponent<TurnBasedBattler>().health -= gameObject.GetComponent<TurnBasedBattler>().activeWeapon.damage;
                selectBox.SetActive(true);
            }
        }
        turnDisp.text = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
