using PGCRefactor.Enum;
using UnityEngine;

namespace PGCRefactor.ModuleAsset.Interface
{
    public interface IAssetsProvider
    {
        public float CurrentProgress{ get; }
        
        public GameObject GetPrefabAsset(PrefabEnum prefabEnum);
    }
}