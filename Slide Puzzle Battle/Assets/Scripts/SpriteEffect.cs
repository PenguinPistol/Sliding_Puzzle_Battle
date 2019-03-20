using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 30f;
    public bool playOnAwake = true;
    public bool loopAnimation = true;
    public bool destroyAuto;

    private bool isPlay = false;
    private int currentFrame;
    private SpriteRenderer sr;

    private IEnumerator animated;

    private void Awake()
    {
        isPlay = false;
        sr = GetComponent<SpriteRenderer>();
        animated = Animated();

        if (playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        if(isPlay == false)
        {
            isPlay = true;

            StartCoroutine(animated);
        }
    }

    public void Stop()
    {
        if(isPlay)
        {
            isPlay = false;
            currentFrame = 0;

            StopCoroutine(animated);
        }
    }

    private IEnumerator Animated()
    {
        do
        {
            yield return null;

            currentFrame = 0;

            while(currentFrame < frames.Length)
            {
                sr.sprite = frames[currentFrame++];

                yield return new WaitForSeconds(1f / fps);
            }
        } while (loopAnimation);

        isPlay = false;

        if(destroyAuto)
        {
            Destroy(gameObject);
        }
    }
}
