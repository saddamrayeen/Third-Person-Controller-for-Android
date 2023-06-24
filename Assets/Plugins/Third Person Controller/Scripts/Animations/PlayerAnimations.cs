using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticleMovements)
    {
        animator.SetFloat("horizontal", horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat("vertical", verticleMovements, 0.1f, Time.deltaTime);
    }
}
