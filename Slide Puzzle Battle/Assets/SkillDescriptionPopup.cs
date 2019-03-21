using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDescriptionPopup : MonoBehaviour
{
    public Text descriptionText;

    private float closeTime;
    private bool isShow = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SkillManager.Instance.descriptionPopup = this;
        gameObject.SetActive(false);
    }

    public void Show(string _description)
    {
        descriptionText.text = _description;

        closeTime = 2f;
        gameObject.SetActive(true);

        if(isShow == false)
        {
            isShow = true;
            animator.Play("SkillDescription_Idle");
            StartCoroutine(CloseCheck());
        }
    }

    private IEnumerator CloseCheck()
    {
        while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            closeTime -= Time.deltaTime;
            yield return null;
        }

        isShow = false;
        gameObject.SetActive(false);
    }
}
