using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler
{
    public int health;

    public int maxHealth;
    public int speed;

    public Item activeItem;
    public Weapon activeWeapon;

    public Battler(int h, int mH, int s)
    {
        this.health = h;
        this.maxHealth = mH;
        this.speed = s;
    }
}

public class Weapon
{
    public int cost;
    public int damage;
    public string name;

    
}

public class Item
{
    public int cost;
    public bool oneTime;
    public string name;
    public Action useFunc;
    public Battler user;

    public Item(int c, bool oT, Action use)
    {
        this.cost = c;
        this.oneTime = oT;
        this.useFunc = use;
    }

    public void Use()
    {
        useFunc();
    }

    public static void LesserHealthPotion(Battler b)
    {
        b.health += 10;
    }
}

public class Battlers
{
    public static Battler fighter = new Battler(100, 100, 13);
    public static Battler mage = new Battler(90, 90, 10);

    public static Battler zombie = new Battler(40, 40, 6);
}
