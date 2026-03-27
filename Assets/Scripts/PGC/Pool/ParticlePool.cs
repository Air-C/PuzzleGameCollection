using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGC.Pool
{
    public class ParticlePool
    {
        
        List<GameObject> pool = new List<GameObject>();
        List<GameObject> active = new ();
        GameObject poolManager;
        private GameContext ctx;

        public ParticlePool(GameContext ctx)
        {
            this.ctx = ctx;
            int size = ctx.assetModule.poolSettings.particlePoolSize;
            GameObject particlePrefab = ctx.assetModule.destroyParticleSystemPrefab;
            poolManager = Object.Instantiate(ctx.assetModule.poolSettings.poolManager);

            for (int i = 0; i < size; i++)
            {
                pool.Add(Object.Instantiate(particlePrefab, poolManager.transform));
            }
        }


        public void ActiveParticle(Vector3 position)
        {
            if (pool.Count <= 0)
            {
                Debug.LogWarning("Pool is empty!");
                //todo 扩容
                return;
            }
            Debug.Log($"Pool Particle:{pool[0]}");
            GameObject particle = pool[0];
            Debug.Log($"Pool Particle:{particle}");
            particle.transform.position = position;
            active.Add(particle);
            pool.RemoveAt(0);
            particle.SetActive(true);
            var ps = particle.GetComponent<ParticleSystem>();
            ps.Play();
        }

        public IEnumerator DeactivateParticle()
        {
            while (active.Count > 0)
            {
                List<GameObject> remove = new List<GameObject>();
                foreach (var particleObj in active)
                {
                    var ps = particleObj.GetComponent<ParticleSystem>();
                    Debug.Log($"Deactive particle isEmitting{ps.isEmitting}");
                    Debug.Log($"Deactive particle particleCount{ps.particleCount}");
                    if (!ps.isEmitting && ps.particleCount == 0)
                    {
                        remove.Add(particleObj);
                    }
                }
                foreach (var particleObj in remove)
                {
                    particleObj.SetActive(false);
                    active.Remove(particleObj);
                    pool.Add(particleObj);
                }
                
                Debug.Log("Deactive particle once time");
                yield return null;
            }
            ctx.hasActiveParticles = false;
        }

        public void DeactivateParticlesForce()
        {
            if (active.Count > 0)
            {
                List<GameObject> remove = new List<GameObject>();
                foreach (var particleObj in active)
                {
                    remove.Add(particleObj);
                }
                foreach (var particleObj in remove)
                {
                    particleObj.SetActive(false);
                    active.Remove(particleObj);
                    pool.Add(particleObj);
                }
                
                Debug.Log("Deactive particle force");
            }
            ctx.hasActiveParticles = false;
        }
        
    }
}