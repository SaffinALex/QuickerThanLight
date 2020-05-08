﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class UpgradeShip : Upgrade
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = icone;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public abstract void StartUpgrade(PlayerShip v);
//Avant la suppression de l'Upgrade.
    public abstract void EndUpgrade(PlayerShip v);
}
