﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed, jumpHeight; //That determine player's max speed
    public Rigidbody rb; //Refer to the player's rigidbody
    public bool snappy; //Choose if we want a snappy vs fluid type movement (testing purposes)
    bool facingRight; //See if player is facing right
    bool isGrounded = false; //Check to is grounded
    Collider[] groundCollision;
    float groundC_rad;
    public LayerMask whatIsGround;
    public Transform groundCheckObj;
    public Text winText;
    private int maxJump = 2;
    int currJump;
    private Animator animator;
    public bool canHide;

    public bool grappleConnection = false;
    private float swingSpeed = 4f;
    public float angleR = 3.927f;
    public float angleL = 3.927f;
    private bool resetVelocity = false;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        facingRight = true;
        //winText.text = "";
        canHide = false;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(isGrounded);
        if (grappleConnection)
        {
            Swing();
        }
        else
        {
            Movement();
            angleR = 3.927f;
            angleL = 3.927f;
            rb.useGravity = true;
            resetVelocity = false;
        }

    }

    // Use FixedUpdate for physics based function
    void FixedUpdate()
    {
        
    }

    void Movement()
    {
            moveHorizontal();
            groundCheck();
            Jump();

    }

    public void knockBack()
    {
        float move;
    }

    //fucntion for moving horizontally
    void moveHorizontal()
    {
        float move;
        
        //See which type of movement we want
        if (!snappy)
        {
            move = Input.GetAxis("Horizontal");
            animator.SetFloat("speed", Mathf.Abs(move));
        }
        else
        {
            move = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("speed", Mathf.Abs(move));
        }

        //move by changing the velocity of the rigidbody
        rb.velocity = new Vector3(move * speed, rb.velocity.y, 0);

        //Check to see if we need to flip the character
        if(move > 0 && !facingRight)
        {
            Flip();
        } else if (move < 0 && facingRight)
        {
            Flip();
        }
    }
    
    //function for flipping
    void Flip()
    {
        facingRight = !facingRight; //switch true to false or false to true
        Quaternion rot = transform.localRotation; //grabbing the z value of the character
        rot.y *= -1; //flip the object
        transform.localRotation = rot; //update the z of the character's scaling
    }

    //function for checking if the player is grounded
    void groundCheck()
    {
        
        groundCollision = Physics.OverlapSphere(groundCheckObj.position, groundC_rad, whatIsGround);
        if(groundCollision.Length> 0)
        {
            isGrounded = true;
            currJump = 0;
            animator.SetBool("isJumping", false);
        } else
        {
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
    }



    //function for jumping 
    void Jump()
    {
        if(Input.GetButtonDown("Jump")  && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector3.up *jumpHeight, 0);
            currJump++;
            
        }


    }

    public bool giveDir()
    {
        if (facingRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Swing()
    {
        GameObject grappleHook = GameObject.Find("grappleHook(Clone)");
        Vector3 centerPoint = grappleHook.transform.position;
        float distance = Vector3.Distance(grappleHook.transform.position, transform.position);
        //angle += swingSpeed * Time.deltaTime;
        var offset = Vector3.zero;
        if (grappleHook.GetComponent<grappleHook>().shootRight)
        {
            //transform.eulerAngles = new Vector3(0, 90, 0);
            angleR += swingSpeed * (1.1f - (Mathf.Abs((angleR - 4.7f) / (4.7f - 3.5f)))) * Time.deltaTime;
            //angleR += swingSpeed * Time.deltaTime;
            offset = new Vector3(Mathf.Cos(angleR), Mathf.Sin(angleR), 0) * distance;
            //if (angleR > 4.7) Debug.Break();
            //transform.eulerAngles = new Vector3(0,90,angleR*(180/Mathf.PI));
            transform.LookAt(grappleHook.transform);
            //transform.rotation *= Quaternion.Euler(90, 0, 0);
            if (transform.position.x - grappleHook.transform.position.x < 0) transform.localRotation *= Quaternion.Euler(90, 0, 0);
            else transform.localRotation *= Quaternion.Euler(-90, -90, -90);
            GetComponent<ShootGrapple>().forceR = offset * angleR;
            if (angleR > 5.9 && transform.position.x - grappleHook.transform.position.x > 0)
            {
                //transform.eulerAngles = new Vector3(0, -90, 0);
                grappleHook.GetComponent<grappleHook>().shootRight = false;
                angleL = 3.5f;
                //Flip();
                transform.localRotation *= Quaternion.Euler(1, -1, 1);
                facingRight = !facingRight;
            }
        }
        else
        {
            //transform.eulerAngles = new Vector3(0, -90, 0);
            angleL += swingSpeed * (1.1f - (Mathf.Abs((angleL - 4.7f) / (4.7f - 3.5f)))) * Time.deltaTime;
            //angleL += swingSpeed * Time.deltaTime;
            offset = new Vector3(-Mathf.Cos(angleL), Mathf.Sin(angleL), 0) * distance;
            //transform.eulerAngles = new Vector3(0,90,-angleL * (180 / Mathf.PI));
            //Debug.Log(angleL);
            transform.LookAt(grappleHook.transform);
            //transform.rotation *= Quaternion.Euler(90, 0, 0);
            if (transform.position.x - grappleHook.transform.position.x > 0) transform.localRotation *= Quaternion.Euler(90, 0, 0);
            else transform.localRotation *= Quaternion.Euler(-90, -90, -90);
            GetComponent<ShootGrapple>().forceL = offset * angleL;
            if (angleL > 5.9 && transform.position.x - grappleHook.transform.position.x < 0)
            {
                //transform.eulerAngles = new Vector3(0, 90, 0);
                grappleHook.GetComponent<grappleHook>().shootRight = true;
                angleR = 3.5f;
                //Flip();
                transform.localRotation *= Quaternion.Euler(1, -1, 1);
                facingRight = !facingRight;
            }
        }
        rb.useGravity = false;
        //Debug.Log(centerPoint - transform.position);
        //rb.MovePosition(transform.position + (offset + centerPoint) * Time.deltaTime);
        rb.MovePosition(centerPoint + offset);
        //transform.position = centerPoint + offset;
        //rb.AddForce((centerPoint - transform.position) * swingSpeed);
        //rb.AddForce(centerPoint + offset);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            //Destroy the gameObject when collide
            Destroy(other.gameObject);
            //Can alternatively set to inactive instead:
            //other.gameObject.SetActive(false);
        }

        /*if (other.gameObject.CompareTag("EndTrigger"))
        {
            winText.text = "Level end!";
        }*/
    }

}

