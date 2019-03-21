using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SkillObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private const float SHOW_DESCRIPTION_TIME = 1f;

    public GameObject lockIcon;
    public GameObject cooldownBackground;
    public Text cooldownText;
    public int index;

    private Button button;
    private bool isCooldown;
    private Skill data;

    public float CurrentCooldown { get { return data.CurrentCooldown; } }

    private float pressedTime;
    private bool isPressed;

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

        if(data.unlockLevel < GameManager.Instance.CompleteLevel)
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

    private IEnumerator CheckPressTime()
    {
        pressedTime = 0f;

        while(isPressed && pressedTime < SHOW_DESCRIPTION_TIME)
        {
            pressedTime += Time.deltaTime;
            yield return null;
        }

        if(pressedTime >= SHOW_DESCRIPTION_TIME)
        {
            SkillManager.Instance.ShowDescription(data, transform.localPosition);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(data.unlockLevel < GameManager.Instance.CompleteLevel)
        {
            isPressed = true;

            StartCoroutine(CheckPressTime());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
