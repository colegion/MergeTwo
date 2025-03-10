using Interfaces;
using Pool;
using UnityEngine;

namespace Helpers
{
    public class ParticleHelper : MonoBehaviour, IInjectable
    {
        private PoolController _poolController;
        public void InjectDependencies()
        {
            _poolController = ServiceLocator.Get<PoolController>();
        }
        
        public void PlayParticleByType(ParticleType particleType, Vector2Int target)
        {
            var particle = _poolController.GetPooledObject(GetPoolableByType(particleType));
            particle.GetGameObject().transform.position = GameController.Instance.GetCell(target.x, target.y).GetTarget().position + Vector3.down * 0.25f;
            particle.GetGameObject().SetActive(true);
            particle.GetGameObject().GetComponent<Particle>().Play();
        }

        private PoolableTypes GetPoolableByType(ParticleType type)
        {
            switch (type)
            {
                case ParticleType.TileMerge:
                    return PoolableTypes.TileMergeParticle;
                case ParticleType.TileSpawn:
                    return PoolableTypes.TileSpawnParticle;
            }

            return PoolableTypes.TileSpawnParticle;
        }
    }

    public enum ParticleType
    {
        TileSpawn,
        TileMerge,
    }
}
