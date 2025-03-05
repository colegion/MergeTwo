using UnityEngine;

namespace Helpers
{
    public class TileParticleHelper : MonoBehaviour
    {
        public void PlayParticleByType(ParticleType particleType)
        {
            
        }
    }

    public enum ParticleType
    {
        TileSpawn,
        TileMerge,
        ReadyToProduce,
    }
}
