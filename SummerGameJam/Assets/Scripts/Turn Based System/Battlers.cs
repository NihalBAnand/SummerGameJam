using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Battler
{
    public int health;

    public int maxHealth;
    public int speed;

    


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

    public Weapon(int c, int d, string n)
    {
        this.cost = c;
        this.damage = d;
        this.name = n;
    }
}

public abstract class Spell
{
    public abstract void Use(TurnBasedBattler user, TurnBasedBattler target);
}

public class Fireball : Spell
{
    public int damage;

    public Fireball(int d)
    {
        this.damage = d;
    }

    public override void Use(TurnBasedBattler user, TurnBasedBattler target)
    {
        target.health -= this.damage;
    }
}

public abstract class Item
{
    public abstract void Use(TurnBasedBattler user);
}


public class HealthPotion : Item
{
    public int potency;
    public HealthPotion(int potent)
    {
        this.potency = potent;
    }
    public override void Use(TurnBasedBattler user)
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

    public static Weapon axe = new Weapon(70, 15, "Axe");
    public static Weapon sword = new Weapon(100, 20, "Sword");
    public static Weapon fist = new Weapon(0, 5, "Fist");
    public static Weapon staff = new Weapon(10, 7, "Staff");

    public static Fireball weakFireball = new Fireball(20);
}
