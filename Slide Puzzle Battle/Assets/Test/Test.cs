using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    public List<Image> images;
    public Text recoveryText;

    private void Start()
    {
        for (int i = 0; i < TestManager.Instance.energy; i++)
        {
            images[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if(TestManager.Instance.IsRecovery)
        {
            recoveryText.text = string.Format("{0:f1}", TestManager.Instance.RecoveryTime);
        }
        else
        {
            recoveryText.text = "";
        }
    }

    public void SetImages(bool _isIncrease)
    {
        int index = TestManager.Instance.energy;

        if (_isIncrease)
        {
            images[index].gameObject.SetActive(true);
        }
        else
        {
            images[index].gameObject.SetActive(false);
        }
    }
}
