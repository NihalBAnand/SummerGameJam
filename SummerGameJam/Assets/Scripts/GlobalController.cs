using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public static GlobalController gc;

    public static int pHealth = 100;
    public static int pMana = 100;
    public static int pStamina = 100;
    public static int pMaxHealth = 100;
    public static int pMaxMana = 100;
    public static int pMaxStamina = 100;
    public static int speed = 3;

    private void Awake()
    {
        //<untouchable>
        if (gc != null)
            GameObject.Destroy(gc);
        else
            gc = this;

        DontDestroyOnLoad(this);
        //</untouchable>

        pHealth = 100;
        pMana = 100;
        pStamina = 100;
        pMaxHealth = 100;
        pMaxMana = 100;
        pMaxStamina = 100;
        speed = 3;
    }
}
