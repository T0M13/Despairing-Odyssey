using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] float sawSpeed = 1f;

    //private const string sawAnimationName = "Rotate_Saw";

    public float SawSpeed { get => sawSpeed; set => sawSpeed = value; }

    private void Awake()
    {
        GetAnimator();
    }

    private void Start()
    {
        //Audio
        if (AudioManager.instance)
            AudioManager.instance.PlayOnObject("saw", gameObject);

        //Rotate
        if (animator)
        {
            SetSpeed();
        }
    }

    private void OnValidate()
    {
        GetAnimator();
        SetSpeed();
    }


    private void GetAnimator()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetSpeed()
    {

        animator.speed = SawSpeed;
    }

}
