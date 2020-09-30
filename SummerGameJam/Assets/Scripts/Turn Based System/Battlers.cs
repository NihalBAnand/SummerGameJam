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

public abstract class Item
{
    public abstract void Use(Battler user);
}


public class HealthPotion : Item
{
    public int potency;
    public HealthPotion(int potent)
    {
        this.potency = potent;
    }
    public override void Use(Battler user)
    {
        if (user.health + this.potency >= user.maxHealth)
        {
            user.health = user.maxHealth;
        }
        else
        {
            user.health += this.potency;
        }
    }
}

public class Battlers
{
    public static Battler fighter = new Battler(100, 100, 13);
    public static Battler mage = new Battler(90, 90, 10);

    public static Battler zombie = new Battler(40, 40, 6);
}
