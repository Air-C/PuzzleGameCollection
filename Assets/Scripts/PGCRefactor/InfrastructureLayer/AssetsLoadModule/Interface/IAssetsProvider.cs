using PGCRefactor.Enum;
using UnityEngine;

namespace PGCRefactor.InfrastructureLayer.AssetsLoadModule.Interface
{
    public interface IAssetsProvider
    {
        public float CurrentProgress{ get; }
        
        public GameObject GetPrefabAsset(PrefabEnum prefabEnum);
    }
}