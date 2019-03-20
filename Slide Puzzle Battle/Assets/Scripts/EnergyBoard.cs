using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBoard : MonoBehaviour
{
    public Image[] images;
    public Text recoveryText;

    private SkillManager sm;

    private void Start()
    {
        sm = SkillManager.Instance;
        sm.energyBoard = this;

        for (int i = 0; i < sm.currentEnergy; i++)
        {
            images[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if(SkillManager.Instance.IsRecovery)
        {
            float minutes = Mathf.Floor(sm.recoveryTime / 60);
            float seconds = Mathf.RoundToInt(sm.recoveryTime % 60);

            recoveryText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            recoveryText.text = "";
        }
    }

    private void OnDestroy()
    {
        sm.energyBoard = null;
    }

    public void CheckCurrentEnergy(bool _isIncrease)
    {
        int index = sm.currentEnergy;

        images[index].gameObject.SetActive(_isIncrease);
    }
}
