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
            particle.GetGameObject().transform.position = new Vector3(target.x, 0.25f, target.y);
            particle.GetGameObject().SetActive(true);
        }

        private PoolableTypes GetPoolableByType(ParticleType type)
        {
            switch (type)
            {
                case ParticleType.TileMerge:
                    return PoolableTypes.TileMergeParticle;
                case ParticleType.TileSpawn:
                    return PoolableTypes.TileSpawnParticle;
                case ParticleType.ReadyToProduce:
                    return PoolableTypes.ReadyToProduceParticle;
            }

            return PoolableTypes.TileSpawnParticle;
        }
    }

    public enum ParticleType
    {
        TileSpawn,
        TileMerge,
        ReadyToProduce,
    }
}
