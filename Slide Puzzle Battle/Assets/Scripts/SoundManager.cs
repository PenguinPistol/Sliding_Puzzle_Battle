using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgm;
    public AudioSource sePrefab;

    public AudioClip[] bgms;
    public AudioClip[] effects;

    // looking into my eyes, you will touch my heart

    public List<IOptionObservable> soundOptions;

    private void Start()
    {
        soundOptions = new List<IOptionObservable>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void PlayBGM(int _index)
    {
        if(GameManager.Instance.GameData.muteBGM)
        {
            bgm.mute = true;
        }

        if(bgm.isPlaying)
        {
            if (bgm.clip.name.Equals(bgms[_index].name))
            {
                return;
            }

            bgm.Stop();
        }

        bgm.clip = bgms[_index];
        bgm.Play();
    }

    public void PlaySE(int _index)
    {
        if(GameManager.Instance.GameData.muteSE)
        {
            return;
        }

        var soundObject = Instantiate(sePrefab, transform);
        soundObject.clip = effects[_index];
        soundObject.Play();

        StartCoroutine(AutoDestroySE(soundObject));
    }

    public AudioSource GetSE(int _index)
    {
        var soundObject = Instantiate(sePrefab, transform);
        soundObject.clip = effects[_index];
        soundObject.loop = true;

        if (GameManager.Instance.GameData.muteSE)
        {
            soundObject.mute = true;
        }

        return soundObject;
    }

    public void MuteSE()
    {
        GameManager.Instance.GameData.muteSE = !GameManager.Instance.GameData.muteSE;

        for (int i = 0; i < soundOptions.Count; i++)
        {
            soundOptions[i].ChangeOption();
        }
    }

    public void MuteBGM()
    {
        GameManager.Instance.GameData.muteBGM = !GameManager.Instance.GameData.muteBGM;

        bgm.mute = GameManager.Instance.GameData.muteBGM;

        for (int i = 0; i < soundOptions.Count; i++)
        {
            soundOptions[i].ChangeOption();
        }
    }

    private IEnumerator AutoDestroySE(AudioSource _se)
    {
        while(_se.isPlaying)
        {
            yield return null;
        }

        Destroy(_se.gameObject);
    }

    public void AddObservable(IOptionObservable _observable)
    {
        if (soundOptions == null || _observable == null)
        {
            return;
        }

        soundOptions.Add(_observable);
    }

    public void RemoveObservable(IOptionObservable _observable)
    {
        if(soundOptions == null || _observable == null)
        {
            return;
        }
        soundOptions.Remove(_observable);
    }
}
