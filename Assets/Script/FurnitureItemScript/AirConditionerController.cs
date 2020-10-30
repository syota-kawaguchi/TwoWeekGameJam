using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConditionerController : MonoBehaviour
{
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void On() {
        if (!animator.enabled) animator.enabled = true;

        animator.SetBool("On", true);
    }

    public void Off() {
        animator.SetBool("On", false);
    }
}
