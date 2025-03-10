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
            
        }

        public void OnFetchFromPool()
        {
            
        }

        public void OnReturnPool()
        {
            
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
