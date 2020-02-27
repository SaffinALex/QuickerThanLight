﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBullet : MonoBehaviour
{
    public int damage;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float posx = transform.position.x;
        float posy = transform.position.y;
        GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(0f, -1f, 0f) * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    public int getDamage()
    {
        return damage;
    }
}