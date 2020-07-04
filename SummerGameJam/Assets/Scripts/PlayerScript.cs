using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Animator anim;
    private int frame = 0;

    private Rigidbody2D rigidbody2D;
    public float speed = 5f;
    Vector2 movement;

    public Image space;
    public Image textBox;
    public Text text;

    public bool signRange = false;
    public string signText;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        space.enabled = false;
        textBox.enabled = false;
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
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
    }

    void FixedUpdate()
    {
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
    }
}
