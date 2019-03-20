using UnityEngine;
using UnityEngine.UI;

public class BGMMuteChangeButton : MonoBehaviour, IOptionObservable
{
    public Text stateText;

    private void Start()
    {
        ChangeOption();
        SoundManager.Instance.AddObservable(this);
    }

    private void OnDestroy()
    {
        SoundManager.Instance.RemoveObservable(this);
    }

    public void ChangeOption()
    {
        if(GameManager.Instance.GameData.muteBGM)
        {
            stateText.text = "OFF";
        }
        else if (GameManager.Instance.GameData.muteBGM == false)
        {
            stateText.text = "ON";
        }
    }

    public void ChangeMute()
    {
        SoundManager.Instance.MuteBGM();
    }
}
