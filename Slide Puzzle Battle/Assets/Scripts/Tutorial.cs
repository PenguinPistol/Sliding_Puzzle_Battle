using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image image;

    public Sprite[] sprites;

    private int index;

    public void Start()
    {
        GameManager.Instance.PauseGame(true);
    }

    public void PrevPage()
    {
        if(index > 0)
        {
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
        GameManager.Instance.PauseGame(false);

        gameObject.SetActive(false);
    }
}
