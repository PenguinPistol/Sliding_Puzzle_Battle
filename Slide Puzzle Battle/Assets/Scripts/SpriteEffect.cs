using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 30f;
    public bool playOnAwake = true;

    private bool isPlay = false;
    private int currentFrame;
    private SpriteRenderer sr;

    private void Start()
    {
        isPlay = false;
        sr = GetComponent<SpriteRenderer>();

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

            StartCoroutine(Animated());
        }
    }

    private IEnumerator Animated()
    {
        yield return null;

        currentFrame = 0;

        while(currentFrame < frames.Length)
        {
            sr.sprite = frames[currentFrame++];

            yield return new WaitForSeconds(1f / fps);
        }

        isPlay = false;
        Destroy(gameObject);
    }
}
