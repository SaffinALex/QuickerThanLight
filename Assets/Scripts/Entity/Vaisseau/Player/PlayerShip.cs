﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerShip : Ship
{
    //Onde du player
    public Onde wave;
    //Dash du player
    public Dash dash;

    [SerializeField] protected GameObject visualSlowDown;

    public int initialShieldLife;
    protected int shieldLife;

    public float initialRecoveryTime;
    protected float recoveryTime;

    public float waveRecovery;

    //IsInvincible = Recovery Time pour éviter de se faire enchainer trop violemment.
    public Animator animator;
    public GameObject bullet;
    public Rigidbody2D myRigidBody;

    //Define the time to achieve the wanted direction
    public float acceleration = 0.2f;

    //Represent the timer to achieve
    float timerChangeVelocity = 0.0f;
    bool needChangeVelocity = false;
    Vector2 lastVelocity;
    Vector2 lastWantedVelocity;

    private bool animationDeadStart = false;

    public void InitConstraints(){
        wave = Instantiate(wave);
        wave.transform.parent = transform;
        wave.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        life = initialLife;
        shieldLife = initialShieldLife;
        recoveryTime = initialRecoveryTime;

        visualSlowDown.SetActive(false);
        wave = Instantiate(wave);
        wave.gameObject.SetActive(true);
        
        dash.initialize();
        for (int i = 0; i < weapons.Count; i++) {
            if(weapons[i] != null){
                weapons[i] = Instantiate(weapons[i]);
                weapons[i].transform.parent = transform;
                weapons[i].transform.position = Vector3.zero;
                weapons[i].Initialize();
                Debug.Log(weapons[i]);
            }
        }
        for (int i = 0; i < upgradeShip.Count; i++)
        {
            if(upgradeShip[i] != null){
                upgradeShip[i].StartUpgrade(this);
                if (shieldLife > maxShieldLife) shieldLife = maxShieldLife;
            }
        }
        UpdateUI();
    }

    void UpdateUI() {
        LevelUIEventManager.GetLevelUI().TriggerPlayerHealthChange((int)life, (int)initialLife, shieldLife);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsDead()) {
            if(!animationDeadStart){
                animationDeadStart = true;
                App.EndGame();
                Destroy(this.gameObject);
            }
        }else{
            ShipBehaviourUpdate();
        }
        wave.transform.position = transform.position;
    }
    
    void ShipBehaviourUpdate(){
        if(recoveryTime < initialRecoveryTime){
            recoveryTime += Time.fixedDeltaTime;
            //Si le temps d'invincibilité est fini alors on arrête l'animation
            if(recoveryTime >= initialRecoveryTime){
                animator.SetBool("isTouch", false);
            }
        }


        //On update les timer des weapons
        for (int i = 0; i < weapons.Count; i++) {
            if(weapons[i] != null && !dash.IsDashing()){
                weapons[i].updateTimer();
                if (Input.GetKey("space"))
                {
                    weapons[i].shoot(transform);
                }
            }
        }

        //Ne pas sortir de l'écran
        Vector3 change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        //Onde tire
        if(Input.GetKey(KeyCode.E)){
            wave.RunOnde();
        }
        //Le Dash se fait seulement sur X.
        if (Input.GetKey(KeyCode.R)) //Dash
        {
            Debug.Log("Dash");
            dash.runDash(change.x);
            lastVelocity = new Vector2(change.x, 0).normalized * speed;
            visualSlowDown.SetActive(false);
        }
        if (!dash.IsDashing())
        {
            //myRigidBody.MovePosition(transform.position + change * speed * Time.deltaTime);

            timerChangeVelocity += Time.deltaTime;
            Vector2 wantedVelocity = new Vector2(change.x, change.y).normalized * speed;
            if(Input.GetKey(KeyCode.Z)){ //Slow down
                wantedVelocity /= 2f;
                visualSlowDown.SetActive(true);
            }else{
                visualSlowDown.SetActive(false);
            }
            Vector2 finalVelocity = wantedVelocity;

            if (wantedVelocity != lastWantedVelocity)
            {
                timerChangeVelocity = 0.0f;
                needChangeVelocity = true;
                lastVelocity = myRigidBody.velocity;
                lastWantedVelocity = wantedVelocity;
            }

            if (needChangeVelocity)
            {
                timerChangeVelocity += Time.deltaTime;
                float percentage = timerChangeVelocity / acceleration;
                percentage = percentage > 1 ? 1 : percentage;
                finalVelocity = Vector2.Lerp(lastVelocity, wantedVelocity, timerChangeVelocity / acceleration);

                if (percentage == 1) needChangeVelocity = false;
            }


            myRigidBody.velocity = finalVelocity;
        }

    }
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            getDamage(10);
        }
        if (col.CompareTag("UpgradeOnde"))
        {
            Debug.Log("UpgradeOnde " + col.gameObject.GetComponent<UpgradeOnde>());
            App.playerManager.getInventory().addUpgradeInventory(col.gameObject.GetComponent<UpgradeOnde>());
            Destroy(col.gameObject);
            App.sfx.PlayEffect("ItemReward", 0.8f);
        }
        if (col.CompareTag("UpgradeDash")) {
            Debug.Log("UpgradeDash " + col.gameObject.GetComponent<UpgradeDash>());
            App.playerManager.getInventory().addUpgradeInventory(col.gameObject.GetComponent<UpgradeDash>());
            Destroy(col.gameObject);
            App.sfx.PlayEffect("ItemReward", 0.8f);
        }
        if (col.CompareTag("UpgradeWeapon")) {
            Debug.Log("UpgradeWeapon " + col.gameObject.GetComponent<UpgradeWeapon>());
            App.playerManager.getInventory().addUpgradeInventory(col.gameObject.GetComponent<UpgradeWeapon>());
            Destroy(col.gameObject);
            App.sfx.PlayEffect("ItemReward", 0.8f);
        }
        if (col.CompareTag("UpgradeShip")) {
            Debug.Log("UpgradeShip " + col.gameObject.GetComponent<UpgradeShip>());
            App.playerManager.getInventory().addUpgradeInventory(col.gameObject.GetComponent<UpgradeShip>());
            //Destroy(col.gameObject);
            App.sfx.PlayEffect("ItemReward", 0.8f);
        }
        if (col.CompareTag("Weapon"))
        {
            Debug.Log(col.gameObject.GetComponent<WeaponPlayer>());
            App.playerManager.getInventory().addWeaponInventory(col.gameObject.GetComponent<WeaponPlayer>());
            Destroy(col.gameObject);App.sfx.PlayEffect("ItemReward", 0.8f);
        }
        if (col.CompareTag("Heart"))
        {
            life += 5;
            life = life > initialLife ? initialLife : life;
            UpdateUI();
            Destroy(col.gameObject);
            App.sfx.PlayEffect("BonusReward", 0.6f);
        }
        if (col.CompareTag("Money"))
        {
            App.playerManager.getInventory().setMoney(App.playerManager.getInventory().getMoney() + 5);
            Destroy(col.gameObject);
            App.sfx.PlayEffect("Coin", 0.3f, 0.25f);
            PanelUIManager.GetPanelUI().ressourcePanel.SetMoney(App.playerManager.getMoney() + App.playerManager.getInventory().getMoney());
        }
        if (col.CompareTag("Shield"))
        {
            Destroy(col.gameObject);
            setShieldLife(shieldLife + 1);
            Debug.Log(shieldLife);
            UpdateUI();
            App.sfx.PlayEffect("BonusReward", 0.6f);
        }
    }
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) {
            getDamage(10);
        }
    }

    public void getDamage(float damage) {
        if (!IsInvincible()) {
            if (shieldLife <= 0) setLife(getLife() - damage);
            else setShieldLife(shieldLife - 1);

            UpdateUI();
            StartInvincibility();
        }
    }

    public int getShieldLife() {
        return shieldLife;
    }

    public void setShieldLife(int s)
    {
        shieldLife = s;
        shieldLife = shieldLife > initialShieldLife ? initialShieldLife : shieldLife < 0 ? 0 : shieldLife;
    }

    public float getRecoveryTime() {
        return recoveryTime;
    }

    public void setRecoveryTime(float s) {
        recoveryTime = s;
    }

    protected void StartInvincibility(){
        recoveryTime = 0f;
        animator.SetBool("isTouch", true);
    }

    public bool IsInvincible(){
        return recoveryTime < initialRecoveryTime || dash.IsDashing();
    }

    public bool IsDead(){
        return life <= 0;
    }
}
