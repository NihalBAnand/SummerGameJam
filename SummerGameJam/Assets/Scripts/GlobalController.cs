using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public static GlobalController i;
    public static int partymembers;
    public static int rangedPlayers;
    public static int turn;
    public static int enemies;
    public static int turnCycle;
    public static int objs;

    void Awake()
    {
        if (!i)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        partymembers = 2;
        enemies = 2;
        turn = 0;
        turnCycle = 0;
        rangedPlayers = 1;
        objs = 1;
    }
    
}

