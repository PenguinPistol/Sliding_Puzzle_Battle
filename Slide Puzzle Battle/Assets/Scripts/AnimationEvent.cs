using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public Transform parent;
    public SpriteEffect[] spriteEffects;

    public void CreateParticle(string _name)
    {
        ParticleManager.Instance.CreateParticle(_name, transform.localPosition);
    }

    public void CrateSpriteEffect(int _index)
    {
        if(parent == null)
        {
            parent = transform;
        }
        var effect = Instantiate(spriteEffects[_index], parent);
    }

    public void PlaySE(int index)
    {
        SoundManager.Instance.PlaySE(index);
    }
}
