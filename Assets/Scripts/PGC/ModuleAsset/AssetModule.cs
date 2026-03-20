using System;
using System.Collections;
using System.Collections.Generic;
using Entities.SO;
using PGC.Entities.Grid.SO;
using PGC.Pool.SO;
using PGC.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace PGC.ModuleAsset
{
    public class AssetModule
    {
        readonly List<SquareSo> squareSoList = new ();
        readonly List<SquareShapeSo> squareShapeSoList = new ();
        public GridSo gridSo;
        public GameObject DestroyParticleSystemPrefab;
        public PoolSettings poolSettings;
        public SystemSettings sysSettings;
        
        public IEnumerator LoadAllAssets()
        {
            yield return LoadAssets<SquareSo>("Square", (squareList) =>
            {
                squareSoList.AddRange(squareList);
            });

            yield return LoadAssets<SquareShapeSo>("Shape", (shapeList) =>
            {
                squareShapeSoList.AddRange(shapeList);
            });
            
            yield return LoadAsset<GridSo>("GridSo", (gridSoAsset) =>
            {
                gridSo = gridSoAsset;
            });
            

            yield return LoadAsset<GameObject>("DestroyParticle", (particle) =>
            {
                DestroyParticleSystemPrefab = particle;
            });
            
            yield return LoadAsset<PoolSettings>("ParticlePoolSettings", (settings) =>
            {
                poolSettings = settings;
            });
            
            yield return LoadAsset<SystemSettings>("PGCSettings", (settings) =>
            {
                sysSettings = settings;
            });
        }
        
        public IEnumerator LoadAssets<T>(string label, Action<IList<T>> callback)
        {
            Debug.Log($"Loading assets: {label}");
            var handle =  Addressables.LoadAssetsAsync<T>(label, null);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"LoadAssets<{label}> failed: {handle.Status}");
                yield break;
            }
            callback?.Invoke(handle.Result);
        }
        
        public IEnumerator LoadAsset<T>(string key, Action<T> callback)
        {
            Debug.Log($"Loading asset: {key}");
            var handle =  Addressables.LoadAssetAsync<T>(key);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"LoadAsset<{key}> failed: {handle.Status}");
                yield break;
            }
            callback?.Invoke(handle.Result);
        }

        public bool ShapeSoTryGetRandom(out SquareShapeSo squareShapeSo)
        {
            if (squareShapeSoList == null || squareShapeSoList.Count == 0)
            {
                squareShapeSo = null;
                return false;
            }
            int index = Random.Range(0, squareShapeSoList.Count);
            squareShapeSo = squareShapeSoList[index];
            return true;
        }

        public bool SquareSoTryGetRandom(out SquareSo squareSo)
        {
            if (squareSoList == null || squareSoList.Count == 0)
            {
                squareSo = null;
                return false;
            }
            int index = Random.Range(0, squareSoList.Count);
            squareSo = squareSoList[index];
            return true;
        }
        
    }
}