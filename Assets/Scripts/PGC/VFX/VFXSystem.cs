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
        
        public void OnLineCleared(LineClearedEvent e)
        {
            foreach (var line in e.lines)
            {
                PlayEffect(line);
            }
            ctx.hasActiveParticles = true;
        }

        void PlayEffect(int line)
        {
            for (int x = 0; x < ctx.grid.GridBorder.x; x++)
            {
                Vector3 pos = ctx.grid.GetWorldPositionByIndex(new Vector2Int(x, line));
                ctx.particlePool.ActiveParticle(pos);
            }
            
        }

        public void Dispose()
        {
            ctx.eventBus.Unsubscribe<LineClearedEvent>(OnLineCleared);
        }
    }
}