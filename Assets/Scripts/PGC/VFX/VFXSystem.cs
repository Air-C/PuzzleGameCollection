using System.Collections.Generic;
using PGC.ModelEvent.Data;
using UnityEngine;

namespace PGC.VFX
{
    public class VFXSystem
    {
        private GameContext ctx;

        public VFXSystem(GameContext ctx)
        {
            this.ctx = ctx;
        }
        
        public void OnLineCleared(SquareClearedEvent e)
        {
            foreach (var index in e.indexes)
            {
                PlayEffect(index);
            }
            ctx.hasActiveParticles = true;
        }

        void PlayEffect(Vector2Int index)
        {
            Vector3 pos = ctx.grid.GetWorldPositionByIndex(index);
            ctx.particlePool.ActiveParticle(pos);
        }

        public void Dispose()
        {
            ctx.eventBus.Unsubscribe<SquareClearedEvent>(OnLineCleared);
        }
    }
}