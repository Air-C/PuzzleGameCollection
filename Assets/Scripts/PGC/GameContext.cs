using System.Collections.Generic;
using Entities;
using Entities.SO;
using PGC.Entities.Grid;
using PGC.ModuleAsset;
using PGC.ModuleInput;
using PGC.Pool;
using Unity.VisualScripting;
using EventBus = PGC.ModelEvent.EventBus;

namespace PGC
{
    public class GameContext
    {
        // entity
        public SquareShapeEntity squareShape;
        public bool CurrentShapeIsReachedBottom = false;
        public List<SquareEntity> NewReacdhedSquare = new List<SquareEntity>();
        public List<SquareEntity> SquaresWaitForDestory = new List<SquareEntity>();
        public GridEntity grid;
        
        //asset
        public AssetModule assetModule;
        public InputModule inputModule;
        
        //pool
        public ParticlePool particlePool;
        
        public EventBus eventBus;
        
        public bool hasActiveParticles = false;
        public bool isPaused = false;
        
        public bool testFlag = false;
        
        public int frameCount = 0;
    }
}