using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class SkillTimeStop : Skill
{
    public override void Activate(params object[] _params)
    {
        GameManager.TimeStop = true;
    }
}
