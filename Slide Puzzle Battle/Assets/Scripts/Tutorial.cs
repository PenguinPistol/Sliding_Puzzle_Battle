using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image image;
    public GameObject playButton;

    public Sprite[] sprites_en;
    public Sprite[] sprites_kr;

    private int index;
    private Sprite[] images;

    public void Start()
    {
        GameManager.Instance.PauseGame(true);

        images = sprites_en;

        if (Application.systemLanguage.ToString().Equals("Korean"))
        {
            images = sprites_kr;
        }

        image.sprite = images[0];
    }

    private void Update()
    {
        if(index == sprites_en.Length - 1)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void PrevPage()
    {
        if(index > 0)
        {
            playButton.SetActive(false);
            image.sprite = images[--index];
        }
    }

    public void NextPage()
    {
        if(index < sprites_en.Length - 1 )
        {
            image.sprite = images[++index];
        }
        else
        {
            Skip();
        }
    }

    public void Skip()
    {
        // 게임 리얼루 시작!
        GameManager.Instance.ChangeState(GameManager.PlayState.Play);

        gameObject.SetActive(false);
    }
}
