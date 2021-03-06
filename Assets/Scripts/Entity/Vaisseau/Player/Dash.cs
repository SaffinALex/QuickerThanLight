﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    // Start is called before the first frame update
    public float initialRecoveryDash;
    public float initialDistance;
    public float initialSpeed;
    public int initialRecharge;

    private int currentRecharge;
    
    private float timerRecoveryDash;
    private float timeDashing;
    private float timerDashing;

    private float directionDash;

    public List<UpgradeDash> upgradeDashes;
    public int nbrUpgradeMax;
    private Rigidbody2D myRigidBody;
    private PlayerShip playerShip;


    void Start()
    {
        currentRecharge = initialRecharge;
        timerRecoveryDash = initialRecoveryDash;
        timerDashing = timeDashing;
        myRigidBody = GetComponent<Rigidbody2D>();
        playerShip = GetComponent<PlayerShip>();
        LevelUIEventManager.GetLevelUI().InitDashCdBar(initialRecharge, initialRecoveryDash);
    }

    public void initialize()
    {
        for (int i = 0; i < upgradeDashes.Count; i++)
        {
            if (upgradeDashes[i] != null)
                upgradeDashes[i].StartUpgrade(this);
        }
    }

    void FixedUpdate(){
        timeDashing = initialDistance / initialSpeed;
        if(timerRecoveryDash < initialRecoveryDash){
            timerRecoveryDash += Time.fixedDeltaTime;
            if(timerRecoveryDash >= initialRecoveryDash && currentRecharge < initialRecharge){
                currentRecharge++;
                if(currentRecharge > initialRecharge) currentRecharge = initialRecharge;
                timerRecoveryDash = 0;
            }
        }
        if(timerDashing < timeDashing){
            timerDashing += Time.fixedDeltaTime;
            myRigidBody.velocity = Vector3.Lerp(Vector3.right * initialSpeed * directionDash, Vector3.right * directionDash * playerShip.speed, timerDashing / timeDashing);
            transform.eulerAngles = Vector3.Lerp(new Vector3(0,60f, 0) * directionDash * -1f, Vector3.zero, Mathf.SmoothStep(0,1, timerDashing / timeDashing));
        }
    }

    public void runDash(float axisX){
        Debug.Log(timerRecoveryDash + " " + initialRecoveryDash);
        if(CanDash() && timerDashing >= timeDashing){
            currentRecharge--;
            if(timerRecoveryDash >= initialRecoveryDash) timerRecoveryDash = 0;
            directionDash = axisX > 0 ? 1 : -1;
            timerDashing = 0f;
            LevelUIEventManager.GetLevelUI().TriggerPlayerDash();
            App.sfx.PlayEffect("Dash", 0.5f);
        }
    }

    public bool CanDash(){
        return currentRecharge > 0;
    }

    public bool IsDashing(){
        return timerDashing < timeDashing;
    }

    public void IncreaseDistance(float d){
        initialDistance += d;
    }

    public void IncreaseSpeed(float s){
        initialSpeed += s;
    }

    public void IncreaseRecoveryDash(float r){
        initialRecoveryDash += r;
    }

    public float getRecoveryDash(){
        return initialRecoveryDash;
    }

    public void addUpgradeDashes(UpgradeDash u)
    {
        upgradeDashes.Add(u);
    }

    public int numberUpgradeCanAdd()
    {
        int cpt = 0;
        for (int i = 0; i < upgradeDashes.Count; i++)
        {
            if (upgradeDashes[i] != null)
                cpt += upgradeDashes[i].getWeight();
        }
        return nbrUpgradeMax - cpt;
    }
}
