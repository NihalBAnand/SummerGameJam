using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public static GlobalController i;
    public static int partymembers;
    public static int turn;
    public static int enemies;

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
    }
    
}

