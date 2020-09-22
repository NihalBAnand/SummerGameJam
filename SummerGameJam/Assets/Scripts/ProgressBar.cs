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

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < targetProgress)
            slider.value += FillSpeed * Time.deltaTime;
        else if (nearMap == true && Input.GetKey(KeyCode.Return) && slider.value < 1)
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
