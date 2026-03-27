using System.Collections.Generic;
using Entities;
using Entities.SO;
using PGC.Entities.Grid;
using PGC.ModuleAsset;
using PGC.ModuleClearLine.Model;
using PGC.ModuleInput;
using PGC.ModuleInventory;
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
        public bool CurrentShapeIsReachedBottom = false;
        public List<SquareEntity> NewReacdhedSquare = new List<SquareEntity>();
        public WaitForDestroySquares SquaresWaitForDestory = new ();
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
        public Queue<WaitForClearModel> waitForClearModelQueue = new Queue<WaitForClearModel>();

        public List<(bool hasItem,GameObject item)> itemsBar = new ();
        public GameObject shapePreviewBar;

    }
}