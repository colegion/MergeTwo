using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Helpers
{
    public class Particle : MonoBehaviour, IPoolable
    {
        [SerializeField] private PoolableTypes type;
        [SerializeField] private List<ParticleSystem> particles;
        
        public void Play()
        {
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }

        public void Stop()
        {
            foreach (var particle in particles)
            {
                particle.Stop();
            }
        }

        public void OnPooled()
        {
            ToggleParticles(false);
        }

        public void OnFetchFromPool()
        {
            ToggleParticles(true);
        }

        public void OnReturnPool()
        {
            ToggleParticles(false);
        }

        private void ToggleParticles(bool toggle)
        {
            foreach (var particle in particles)
            {
                particle.gameObject.SetActive(toggle);
            }
        }
        
        public PoolableTypes GetPoolableType()
        {
            return type;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
