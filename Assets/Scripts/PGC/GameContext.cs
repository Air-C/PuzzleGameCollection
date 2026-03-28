using System.Collections.Generic;
using Entities;
using Entities.SO;
using PGC.Entities.Grid;
using PGC.Enum;
using PGC.ModuleAsset;
using PGC.ModuleClearLine.Model;
using PGC.ModuleInput;
using PGC.ModuleInventory;
using PGC.ModuleItem.Model;
using PGC.Pool;
using PGC.System;
using UnityEngine;
using UnityEngine.UI;
using EventBus = PGC.ModelEvent.EventBus;
using SaveData = PGC.ModuleInventory.SO.SaveData;

namespace PGC
{
    public class GameContext
    {
        // entity
        public SquareShapeEntity currentSquareShape;
        public bool currentShapeIsReachedBottom = false;
        public List<SquareEntity> newReachedSquare = new List<SquareEntity>();
        public WaitForDestroySquares squaresWaitForDestroy = new ();
        public GridEntity grid;

        public SaveData saveData;
        public SquareShapeSo previewShape;
        
        //asset
        public AssetModule assetModule;
        public InputModule inputModule;
        
        //pool
        public ParticlePool particlePool;
        
        // event
        public EventBus eventBus;
        
        public bool hasActiveParticles = false;
        
        public bool testFlag = false;
        public int frameCount = 0;
        
        //state
        public GameSystemState gameSystemState;
        
        public InventoryLocalData inventoryLocalData;
        
        public int totalScore = 0;

        public List<ItemBarModel> itemsBar = new ();
        public GameObject shapePreviewBar;

    }
}