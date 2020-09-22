using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Animator anim;
    private int frame = 0;
    public float speed = 5f;
    Vector2 movement;

    public Rigidbody2D rigidbody2D;
    public Image space;
    public Image textBox;
    public Text text;

    public Slider progressBar; 

    public bool signRange = false;
    public string signText;

    public int health;
    public int mana;
    public int stamina;
    public int maxHealth;
    public int maxMana;
    public int maxStamina;
    public int battleSpeed;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
      

        space.enabled = false;
        textBox.enabled = false;
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("Assets/Scenes/Battle System Testing.unity");
        }
        if (movement.x != 0 || movement.y != 0)
        {
            
            if (movement.x > 0)
            {
                anim.Play("PlayerWalkRight");
                frame = 1;
            }
            else if (movement.x < 0)
            {
                anim.Play("PlayerWalkLeft");
                frame = 2;
            }
            else if (movement.y > 0)
            {
                anim.Play("PlayerWalkUp");
                frame = 3;
            }
            else if (movement.y < 0)
            {
                anim.Play("PlayerWalkDown");
                frame = 0;
            }
        }
        else
        {
            switch (frame)
            {
                case 0:
                    anim.Play("PlayerIdleDown");
                    break;
                case 1:
                    anim.Play("PlayerIdleRight");
                    break;
                case 2:
                    anim.Play("PlayerIdleLeft");
                    break;
                case 3:
                    anim.Play("PlayerIdleUp");
                    break;
            }
        }

        //logic for detecting if sign is accessible and displaying the text if space is pressed
        if (signRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!textBox.enabled)
                {
                    textBox.enabled = true;
                    text.enabled = true;
                    text.text = signText;
                }
                else
                {
                    textBox.enabled = false;
                    text.enabled = false;
                }
                
            }
        }
        else
        {
            textBox.enabled = false;
            text.enabled = false;
        }

        //debug for static data
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(1);
        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log(health);
    }

    void FixedUpdate()
    {
        //don't touch unless it's broken - movement
        rigidbody2D.MovePosition(rigidbody2D.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sign")
        {
            space.enabled = true;
            signText = collision.GetComponent<SignController>().text;
            signRange = true;
        }
        if (collision.tag == "mapper")
        {
            progressBar.GetComponent<ProgressBar>().holdFill();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "sign")
        {
            space.enabled = false;
            signRange = false;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "door")
        {
            if (collision.collider.GetComponent<DoorController>().reverse)
                rigidbody2D.position = new Vector2(collision.collider.GetComponent<DoorController>().door.transform.position.x, collision.collider.GetComponent<DoorController>().door.transform.position.y + 1.5f);
            else
                rigidbody2D.position = new Vector2(collision.collider.GetComponent<DoorController>().door.transform.position.x, collision.collider.GetComponent<DoorController>().door.transform.position.y - 1.5f);
        }

        //debug for static data
       
    }
}
