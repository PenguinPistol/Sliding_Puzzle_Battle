using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image image;
    public GameObject playButton;

    public Sprite[] sprites;

    private int index;

    public void Start()
    {
        GameManager.Instance.PauseGame(true);
    }

    private void Update()
    {
        if(index == sprites.Length - 1)
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
            image.sprite = sprites[--index];
        }
    }

    public void NextPage()
    {
        if(index < sprites.Length - 1 )
        {
            image.sprite = sprites[++index];
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
