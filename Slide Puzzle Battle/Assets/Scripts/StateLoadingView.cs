using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLoadingView : LoadingView
{
    private void Started()
    {
        StartCoroutine(PlayAnimation("StateClose"));
    }
    
    private void Finished()
    {
        StartCoroutine(PlayAnimation("StateChange"));
    }

    private IEnumerator PlayAnimation(string _animationName)
    {
        animator.Play(_animationName);

        while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
    }
}
