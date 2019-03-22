using UnityEngine;
using UnityEngine.UI;

public class SEMuteChangeButton : MonoBehaviour, IOptionObservable
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
        if(GameManager.Instance.GameData.muteSE)
        {
            stateText.text = "OFF";
        }
        else if (GameManager.Instance.GameData.muteSE == false)
        {
            stateText.text = "ON";
        }
    }

    public void ChangeMute()
    {
        SoundManager.Instance.MuteSE();
    }
}
