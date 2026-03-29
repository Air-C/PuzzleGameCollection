using UnityEngine;

namespace PGC.Pool.SO
{
    
    [CreateAssetMenu(fileName = "ParticlePoolSettings", menuName = "PGC/ParticlePoolSettings")]
    public class ParticlePoolSettings : ScriptableObject
    {
        
        public int particlePoolSize;
        public GameObject poolManager;

    }
}