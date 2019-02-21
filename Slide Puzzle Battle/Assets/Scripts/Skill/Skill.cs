using UnityEngine;

public abstract class Skill
{
    public Sprite icon;
    public string name;
    public string description;
    public int cost;

    public Skill(Sprite _icon, string _name, string _description, int _cost)
    {
        icon = _icon;
        name = _name;
        description = _description;
        cost = _cost;
    }

    public abstract void Activate();
}
