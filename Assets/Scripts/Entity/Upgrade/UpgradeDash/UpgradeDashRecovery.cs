﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDashRecovery : UpgradeDash
{
    public float recoveryBonus;

    override
    public void StartUpgrade(Dash d){
        d.setRecoveryDash(recoveryBonus);
    }
//Avant la suppression de l'Upgrade.

}
