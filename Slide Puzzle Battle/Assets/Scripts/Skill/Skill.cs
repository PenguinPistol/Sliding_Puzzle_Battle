using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Skill : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite sprite;
    public int unlockLevel;
    public float cooldown;
    public float value;
    public int cost;
    public bool isCooldown;

    private float currentCooldown = 0;
    public float CurrentCooldown { get { return currentCooldown; } }

    public abstract void Activate(params object[] _params);

    public IEnumerator Cooldown(GameObject _cooldownObject, Text _cooldownText)
    {
        currentCooldown = cooldown;

        isCooldown = true;
        _cooldownObject.SetActive(true);

        while (currentCooldown > 0)
        {
            while (GameManager.Instance.IsPlaying == false)
            {
                yield return null;
            }

            currentCooldown -= Time.deltaTime;

            _cooldownText.text = string.Format("{0:f0}", currentCooldown);

            yield return null;
        }

        isCooldown = false;
        _cooldownObject.SetActive(false);
    }
}
