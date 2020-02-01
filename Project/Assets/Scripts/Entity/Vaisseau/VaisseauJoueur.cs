﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaisseauJoueur : Vaisseau
{
    public Onde onde;
    public Dash dash;
    public int vieShield;
    public int recoveryTime;

    //IsInvincible = Recovery Time pour éviter de se faire enchainer trop violemment.
    private bool isInvincible;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 change = Vector3.zero;
        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");
        //Le Dash se fait seulement sur X.
        if(dash.getCanDash() && Input.GetKeyDown("space") && change.x != 0){
            Debug.Log("Dash");
            float posx = dash.runDash(change.x);
            if(change.x < 0)
                StartCoroutine("FlippingLeft");
            if(change.x > 0 )
             StartCoroutine("FlippingRight");
           // GetComponent<Rigidbody>().MovePosition(transform.position + p);
            transform.position = new Vector3(transform.position.x + posx, transform.position.y, transform.position.z);
        }
        else{
            transform.position = new Vector3(transform.position.x + change.x*speed, transform.position.y + change.y*speed, transform.position.z);
            //GetComponent<Rigidbody>().MovePosition(transform.position + change * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D col){
    	if(col.gameObject.CompareTag("Bullet") && !isInvincible){
            //life - col.gemeObject.getDmg();
            StartCoroutine("InvincibiltyCount");
    	}
    }

    private IEnumerator InvincibiltyCount(){
        isInvincible = true;
        yield return new WaitForSeconds(recoveryTime);
        isInvincible = false;
    }

    private IEnumerator FlippingRight(){
        animator.SetBool("isFlipRight", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("isFlipRight", false);
    }

    private IEnumerator FlippingLeft(){
        animator.SetBool("isFlipLeft", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("isFlipLeft", false);
    }
}
