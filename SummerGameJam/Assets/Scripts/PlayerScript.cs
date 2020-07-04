using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Animator anim;

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
                anim.Play("PlayerWalkLeft");
            }
            else if (movement.x < 0)
            {
                anim.Play("PlayerWalkLeft");
            }
            else if (movement.y > 0)
            {
                anim.Play("PlayerWalkUp");
            }
            else if (movement.y < 0)
            {
                anim.Play("PlayerWalkDown");
            }
        }
        else
        {
            anim.Play("PlayerIdle");
            
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
}
