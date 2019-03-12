using UnityEngine;
using System.Collections;

public class Skill
{
    public enum Type
    {
        SKILL_CHANGE_TILE_TYPE , SKILL_REINFORCE, SKILL_GENARAL
    }

    public string skillType;
    public string name;
    public string description;
    public int unlockLevel;
    public float coolDown;
    public float value;
    public int cost;

    private bool isCoolDown;

    public void Activate()
    {
        /*
        if(isCoolDown || unlockLevel > GameManager.Instance.completeLevel)
        {
            return;
        }

        switch((Type)System.Enum.Parse(typeof(Type), skillType))
        {
            case Type.SKILL_CHANGE_TILE_TYPE:
                // 타일변경
                switch((int)value)
                {
                    case 1:
                        GameManager.Instance.ChangeTile(typeof(SwordTile));
                        Debug.Log("검 타일 생성");
                        break;
                    case 2:
                        GameManager.Instance.ChangeTile(typeof(ArrowTile));
                        Debug.Log("활 타일 생성");
                        break;
                    case 3:
                        GameManager.Instance.ChangeTile(typeof(BombTile));
                        Debug.Log("폭탄 타일 생성");
                        break;
                }
                break;
            case Type.SKILL_REINFORCE:
                GameManager.Instance.reinforceScope = (int)value;
                Debug.Log("타일 강화");
                // 타일강화
                break;
            case Type.SKILL_GENARAL:
                // 그 외 스킬
                Debug.Log("일반 스킬 사용");
                break;
        }

        // 쿨다운 사용안함
        //GameManager.Instance.StartCoroutine(CoolDown());
        */
    }

    public IEnumerator CoolDown()
    {
        float 시간 = 0f;

        isCoolDown = true;

        while (시간 <= coolDown)
        {
            시간 += Time.deltaTime;

            yield return null;
        }

        isCoolDown = false;
    }
}
