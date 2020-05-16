﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Ship player;
    private int money;
    private int score;
    private List<WeaponPlayer> Weapons;
    private List<UpgradeDash> UpgradeDashes;
    private List<UpgradeOnde> UpgradesOnde;
    private List<UpgradeShip> UpgradeShip;
    private List<UpgradeWeapon> UpgradeWeapon;

    public List<WeaponPlayer> getWeapons(){
        return this.Weapons;
    }
    public List<UpgradeShip> getUpgradeShip(){
        return this.UpgradeShip;
    }
    public List<UpgradeOnde> getUpgradesOnde()
    {
        return this.UpgradesOnde;
    }
    public List<UpgradeDash> getUpgradeDashes(){
        return this.UpgradeDashes;
    }
    public List<UpgradeWeapon> getUpgradeWeapon(WeaponPlayer p){
        return this.UpgradeWeapon;
    }
    public void addUpgradeInventory(UpgradeDash d){
        UpgradeDashes.Add(d);
    }
    public void addUpgradeInventory(UpgradeWeapon d){
        UpgradeWeapon.Add(d);
    }
    public void addUpgradeInventory(UpgradeShip d){
        UpgradeShip.Add(d);
    }
    public void addUpgradeInventory(UpgradeOnde d)
    {
        UpgradesOnde.Add(d);
    }
    public void addWeaponInventory(WeaponPlayer d){
        Weapons.Add(d);
    }

    public int getMoney(){
        return money;
    }
    public void setMoney(int m){
        money = m;
    }

    public int getScore(){
        return score;
    }
    public void setScore(int m){
        score = m;
    }

    public List<Upgrade> getUpgrades()
    {
        List<Upgrade> upgrades = new List<Upgrade>();

        upgrades.AddRange(UpgradeDashes);
        upgrades.AddRange(UpgradeShip);
        upgrades.AddRange(UpgradeWeapon);
        upgrades.AddRange(UpgradesOnde);

        return upgrades;
    }

    public void flushItemInventory()
    {
        Weapons.Clear();
        UpgradeDashes.Clear();
        UpgradeShip.Clear();
        UpgradeWeapon.Clear();
        UpgradesOnde.Clear();
    }
}
