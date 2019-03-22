using UnityEngine;
using System.Collections;

public class LoadingView : MonoBehaviour
{
    public Animator animator;

    public AnimationClip startClip;
    public AnimationClip finishClip;

    public IEnumerator StartAnimation()
    {
        animator.Play(startClip.name);
        float time = 0f;
        
        while (time < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FinishAnimation()
    {
        animator.Play(finishClip.name);
        float time = 0f;

        while (time < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;
            yield return null;
        }
    }
}
