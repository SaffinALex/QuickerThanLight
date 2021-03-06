﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarControl : MonoBehaviour
{

    private float healthPercent = 1;
    private float currentGhostPercent = 1;

    public float ghostDelay = 1;
    public float currentGhostDelay = 0;
    public float ghostFillSpeed = 0.003f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Find("Main Slider").GetComponent<Slider>().value = healthPercent;
        gameObject.transform.Find("Ghost Slider").GetComponent<Slider>().value = currentGhostPercent;
        gameObject.transform.Find("ParticlesSlider").GetComponent<Slider>().value = currentGhostPercent;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentGhostDelay <= 0 && healthPercent < currentGhostPercent){
            currentGhostPercent -= ghostFillSpeed;
            gameObject.transform.Find("Ghost Slider").GetComponent<Slider>().value = currentGhostPercent;
            gameObject.transform.Find("ParticlesSlider").GetComponent<Slider>().value = currentGhostPercent;
        }
        else if (currentGhostDelay > 0)
            currentGhostDelay -= Time.deltaTime;
    }

    public void setHealthPercent(int health, int maxHealth){
        float healthPercent = (float) (((float) health)/((float) maxHealth));

        if(healthPercent < 0)
            healthPercent = 0;
        if(healthPercent > 1)
            healthPercent = 1;
        if(this.healthPercent < healthPercent){
            gameObject.transform.Find("Ghost Slider").GetComponent<Slider>().value = healthPercent;
            gameObject.transform.Find("ParticlesSlider").GetComponent<Slider>().value = healthPercent;
            currentGhostPercent = healthPercent;
        }
            
        this.healthPercent = healthPercent;
        gameObject.transform.Find("Main Slider").GetComponent<Slider>().value = this.healthPercent;

        gameObject.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = (this.healthPercent*100).ToString("0");
        currentGhostDelay = ghostDelay;
    }

    public void setHealth(int health){
        gameObject.transform.Find("HealthText").GetComponent<TextMeshProUGUI>().text = (this.healthPercent*100).ToString("0");
    }

    public void setShields(int nbShields){
        if(nbShields == 0){
            gameObject.transform.Find("HealthText").gameObject.SetActive(true);
            gameObject.transform.Find("ShieldBar").GetComponentInChildren<CanvasGroup>().alpha = 0;
        } else {
            if (gameObject.transform.Find("HealthText").gameObject.activeSelf){
                gameObject.transform.Find("HealthText").gameObject.SetActive(false);
            }
            if (!gameObject.activeSelf){
                gameObject.transform.Find("ShieldBar").gameObject.SetActive(true);
            }

            gameObject.transform.GetComponentInChildren<ShieldBarControl>().setNbShield(nbShields);
            gameObject.transform.Find("ShieldBar").GetComponentInChildren<CanvasGroup>().alpha = 1;
        }
    }
}
