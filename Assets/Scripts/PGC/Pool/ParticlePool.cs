using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGC.Pool
{
    class PoolParticle
    {
        public GameObject particle;
        public ParticleSystem ps;
    }
    
    public class ParticlePool
    {
        
        Stack<PoolParticle> pool = new ();
        Stack<PoolParticle> active = new ();
        GameObject poolManager;
        private GameContext ctx;

        public ParticlePool(GameContext ctx)
        {
            this.ctx = ctx;
            int size = ctx.assetModule.poolSettings.particlePoolSize;
            GameObject particlePrefab = ctx.assetModule.destroyParticleSystemPrefab;
            poolManager = Object.Instantiate(ctx.assetModule.poolSettings.poolManager);
            GameObject obj;
            for (int i = 0; i < size; i++)
            {
                obj = Object.Instantiate(particlePrefab, poolManager.transform);
                pool.Push(new PoolParticle()
                {
                    particle = obj,
                    ps = obj.GetComponent<ParticleSystem>(),
                });
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
            PoolParticle particle = pool.Pop();
            particle.particle.transform.position = position;
            active.Push(particle);
            particle.particle.SetActive(true);
            particle.ps.Play();
        }

        public IEnumerator DeactivateParticle()
        {
            PoolParticle particle = active.Peek();
            var ps = particle.ps;
            while (ps.isEmitting || ps.particleCount > 0)
            {
                yield return null;
            }
            while (active.Count > 0)
            {
                PoolParticle removePar = active.Pop();
                removePar.particle.SetActive(false);
                pool.Push(removePar);
            }
            ctx.hasActiveParticles = false;
        }

        public void DeactivateParticlesForce()
        {
            while (active.Count > 0)
            {
                pool.Push(active.Pop());
            }
            ctx.hasActiveParticles = false;
        }
        
    }
}