using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillObject : MonoBehaviour
{
    public GameObject lockIcon;
    public GameObject cooldownBackground;
    public Text cooldownText;
    public int index;

    private Button button;
    private bool isCooldown;
    private Skill data;

    public float CurrentCooldown { get { return data.CurrentCooldown; } }
    
    private void Start()
    {
        data = SkillManager.Instance.skills[index];

        if(data.isCooldown)
        {
            data.isCooldown = false;
        }

        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if(data.isCooldown
            || SkillManager.Instance.currentEnergy < data.cost
            || GameManager.Instance.IsPlaying == false)
            {
                return;
            }

            SkillManager.Instance.UseSkill(index);
            StartCoroutine(data.Cooldown(cooldownBackground, cooldownText));
        });

        if(data.unlockLevel <= GameManager.Instance.CompleteLevel)
        {
            // 언락
            lockIcon.SetActive(false);
            button.enabled = true;
        }
        else
        {
            button.enabled = false;
        }
    }
}
