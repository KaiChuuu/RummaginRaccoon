using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomizeAnimationStart : MonoBehaviour
{
    private Animator animator;
    public float animatorSpeedMin;
    public float animatorSpeedMax;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = Random.Range(animatorSpeedMin, animatorSpeedMax);

            // Get the length of the animation
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = state.length;

            // Randomize the start frame
            float randomStartTime = Random.Range(0f, animationLength);
            animator.Play(state.fullPathHash, 0, randomStartTime / animationLength);
        }
        else
        {
            Debug.LogWarning("Animator component not found on " + gameObject.name);
        }
    }
}
