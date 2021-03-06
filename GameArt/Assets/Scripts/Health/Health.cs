﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {


    public int maxHealth; 
    public Image HealthBar;
    public float regenTimer; //set how fast the regen is 
    float timer;
    public int curHealth, regenRate, holdHP; //regenRate is the rate at which health regens at, holdHP keeps track of current health changes
    private Animator animator;

    public float knockbackForce;
    public float knockbackTime;
    private float knockbackCounter;
    public Rigidbody rb;

	// Use this for initialization
	void Start () {
        timer = regenTimer;
        curHealth = maxHealth;
        holdHP = curHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if(holdHP != curHealth) { // Checks for current health change and updates the health bar.
            holdHP = curHealth;
            if (gameObject.tag == "Player")
            {
                HealthBar.fillAmount = (float)holdHP / 100;
            }
        }

        //Debug.Log(HealthBar.fillAmount);
        if(curHealth < maxHealth && curHealth >0) //if player's health is not at full and still alive, then they're able to regen health
        {
            Regen();
        } else if (curHealth <= 0) //death when player's health reaches zero
        {
            Death();
        }
	}

    //Function for taking damage
    public void takeDamage(int dam) //function is made to public so that other scripts can access it 
    {
        curHealth -=  dam;
        knockBack();
        if (curHealth < 0)
        {
            animator.SetTrigger("Death");
            Death();
        }
        if(gameObject.tag == "Enemy")
        {
            Audio.PlaySound("EnemyHurt");
        }
        else if (gameObject.tag == "Player")
        {
            Audio.PlaySound("PlayerHurt");
        }
    }

    public void knockBack()
    {
        float speed = 5;
        float move = -20;
        rb.velocity = new Vector3(move * speed, rb.velocity.y, 0);
    }


    //Function for health regen
    void Regen()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            curHealth += regenRate;
            timer = regenTimer;
        }
    }

    //Function for death
    public void Death()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 1.5f);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
