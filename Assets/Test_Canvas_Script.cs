using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Canvas_Script : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip clip;
   
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Cheer_Anim");
        animator.Play("Confetti_Anim");
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
