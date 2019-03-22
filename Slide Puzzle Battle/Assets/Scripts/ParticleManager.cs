using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class ParticleManager : Singleton<ParticleManager>
{
    private Dictionary<string, ParticleSystem> particles;

    public List<DictionaryParticle> items = new List<DictionaryParticle>();
    public int size = 0;

    public List<ParticleSystem> createdParticles;

    private void Start()
    {
        particles = new Dictionary<string, ParticleSystem>();
        createdParticles = new List<ParticleSystem>();

        for (int i = 0; i < items.Count; i++)
        {
            particles.Add(items[i].key, items[i].value);
        }
    }

    private void Update()
    {
        for(int i = 0; i < createdParticles.Count; i++)
        {
            var particle = createdParticles[i];

            if(particle.IsAlive(true) == false)
            {
                createdParticles.Remove(particle);
                Destroy(particle.gameObject);
            }
        }
    }

    public void CreateParticle(string _name, Vector3 _position)
    {
        var particle = Instantiate(particles[_name].gameObject, _position, Quaternion.identity).GetComponent<ParticleSystem>();

        createdParticles.Add(particle);
    }

    private IEnumerator AutoDestroy(ParticleSystem _particle)
    {
        while(_particle.IsAlive(true))
        {
            yield return null;
        }

        Destroy(_particle.gameObject);
    }
}
