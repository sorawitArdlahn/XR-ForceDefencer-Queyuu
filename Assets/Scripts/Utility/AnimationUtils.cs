using System.Collections;
using UnityEngine;

public static class AnimationUtils
{
    public static IEnumerator WaitForAnimation(Animator animator, string animationName)
    {
        animator.SetTrigger(animationName);

        // Wait until the animation has finished playing
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
    }
}
