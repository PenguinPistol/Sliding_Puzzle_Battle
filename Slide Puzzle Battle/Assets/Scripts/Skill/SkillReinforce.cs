using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class SkillReinforce : Skill
{
    public override void Activate(params object[] _params)
    {
        GameManager.Reinforce = 2;
    }
}
