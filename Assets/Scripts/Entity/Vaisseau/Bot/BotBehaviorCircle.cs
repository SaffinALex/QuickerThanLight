﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBehaviorCircle : EntitySpaceShipBehavior
{
    private Rigidbody2D r2d;
    private int direction;

    public GameObject bullet;
    public float life;
    public float speedMove;
    public float scrolling;
    public float speedShoot;
    public Animator animator;
    public bool isDead = false;
    public bool isShooting;
    public bool isMoving;
    //weapon associé à un type de bullet

    // Start is called before the first frame update
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        direction = 0;

        //r2d.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            Destroy(this.gameObject);
        }

        move();
        shoot();
    }

    public void move()
    {
        /*
        if (Input.GetKey("d"))
        {
            r2d.velocity = new Vector2(speed, 0);
        }
        else if (Input.GetKey("q"))
        {
            r2d.velocity = new Vector2(-speed, 0);
        }
        else
        {
            r2d.velocity = new Vector2(0, 0);
        }
        */
        if (!isMoving)
        {
            StartCoroutine("Move");
            if (direction == 1) direction = 2;
            else if (direction == 2) direction = 3;
            else if (direction == 3) direction = 0;
            else direction = 1;
        }
        if (direction == 0)
        {
            r2d.velocity = new Vector2(-speedMove, -speedMove - scrolling);
        }
        else if (direction == 1)
        {
            r2d.velocity = new Vector2(speedMove, -speedMove - scrolling);
        }
        else if (direction == 2)
        {
            r2d.velocity = new Vector2(speedMove, speedMove - scrolling);
        }
        else
        {
            r2d.velocity = new Vector2(-speedMove, speedMove - scrolling);
        }
    }

    public void shoot()
    {
        if (isShooting)
        {
            StartCoroutine("Shoot");
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            life -= collision.gameObject.GetComponent<PlayerBullet>().getDamage();
            if (life <= 1)
            {
                animator.SetBool("isDead", true);
            }
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator Shoot()
    {
        isShooting = false;
        yield return new WaitForSeconds(speedShoot);
        isShooting = true;
    }

    private IEnumerator Move()
    {
        isMoving = true;
        yield return new WaitForSeconds(1.0f);
        isMoving = false;

    }
}