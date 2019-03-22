using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.PlugStudio.Patterns;

public class SkillManager : Singleton<SkillManager>
{
    public List<Skill> skills;
    public EnergyBoard energyBoard;
    public SkillDescriptionPopup descriptionPopup;

    public int currentEnergy;
    public float recoveryTime;
    public float elapseRecoveryTime;

    private bool doRecovery;

    public bool IsRecovery { get { return doRecovery; } }

    public void Init()
    {
        doRecovery = false;

        if(currentEnergy < GameConst.MaxEnergy)
        {
            StartCoroutine(RecoveryEnergy());
        }
    }

    public void UseSkill(int _index)
    {
        if(currentEnergy < skills[_index].cost || GameManager.Instance.IsPlaying == false)
        {
            return;
        }

        SoundManager.Instance.PlaySE(7);

        currentEnergy -= skills[_index].cost;
        skills[_index].Activate();
        energyBoard.CheckCurrentEnergy(false);

        if (doRecovery == false)
        {
            StartCoroutine(RecoveryEnergy());
        }
    }

    private IEnumerator RecoveryEnergy()
    {
        recoveryTime = GameConst.Cooldown_EnergyRecovery - elapseRecoveryTime;

        doRecovery = true;

        while (recoveryTime > 0f)
        {
            recoveryTime -= 1f;

            yield return new WaitForSeconds(1f);
        }

        currentEnergy++;
        energyBoard.CheckCurrentEnergy(true);

        if(elapseRecoveryTime > 0)
        {
            elapseRecoveryTime = 0f;
        }

        if (currentEnergy < GameConst.MaxEnergy)
        {
            StartCoroutine(RecoveryEnergy());
        }
        else
        {
            doRecovery = false;
        }
    }

    // 테스트용
    public void AddEnergy()
    {
        if(currentEnergy == GameConst.MaxEnergy)
        {
            return;
        }

        if(doRecovery)
        {
            recoveryTime = 0;
        }
        else
        {
            currentEnergy++;
            energyBoard.CheckCurrentEnergy(true);
        }
    }

    public void ShowDescription(Skill _data, Vector3 _position)
    {
        Vector3 position = descriptionPopup.transform.localPosition;

        position.x = _position.x / 2;

        descriptionPopup.transform.localPosition = position;
        descriptionPopup.Show(_data.description);
    }
}
