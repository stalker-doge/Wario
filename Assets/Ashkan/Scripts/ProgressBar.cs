using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider progressBar;
    public float duration;
    
    private bool isFinished;
    private float timer;

    private void Start()
    {
        isFinished = true;
    }

    private void Update()
    {
        if (isFinished)
        {
            
            timer += Time.deltaTime;
            float progress = timer / duration;
            progressBar.value  = Mathf.Clamp01(progress);

            if (progress >= 1)
            {
                isFinished = false;
                
               
                print("progress bar finished");          // here we can get any variable related to next level
            }

        }
        

    }
}