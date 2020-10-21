using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider slider;

    private float FillSpeed = 0.1f;
    private float targetProgress = 0;
    private bool nearMap = false;
    PlayerScript playerScript;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    void Start()
    {
        playerScript = GameObject.Find("PlayerScript").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < targetProgress)
            slider.value += FillSpeed * Time.deltaTime;
        else if (nearMap == true && Input.GetKey(KeyCode.Return) && slider.value < 1 && playerScript.signRange == true)
        {
                slider.value += FillSpeed * Time.deltaTime;
        }
    }
    public void IncrementProgress(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }
    public void holdFill()
    {
        nearMap = true;
    }

}
