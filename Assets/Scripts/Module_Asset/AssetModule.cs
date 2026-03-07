using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PGC {

    public class AssetModule {

        Dictionary<ShapeType, ShapeSO> shapes;
        AsyncOperationHandle shapeHandle;

        SquareEntity squareEntityPrefab;
        AsyncOperationHandle squareEntityPrefabHandle;

        public AssetModule() {
            shapes = new Dictionary<ShapeType, ShapeSO>();
        }

        public IEnumerator LoadAllIE() {
            yield return Shape_Load();
            yield return SquareEntityPrefab_Load();
        }

        public void UnloadAll() {
            if (shapeHandle.IsValid()) {
                Addressables.Release(shapeHandle);
            }
            if (squareEntityPrefabHandle.IsValid()) {
                Addressables.Release(squareEntityPrefabHandle);
            }
        }

        #region Shape
        IEnumerator Shape_Load() {
            string label = "Shape";
            AssetLabelReference labelReference = new AssetLabelReference();
            labelReference.labelString = label;
            // handle 是非托管内存, 不由GC管理
            var handle = Addressables.LoadAssetsAsync<ShapeSO>(labelReference, null);
            yield return handle;
            if (!handle.IsDone) {
                Debug.LogError($"Failed to load SquareSO with label {label}");
                yield break;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded) {
                Debug.LogError($"Failed to load SquareSO with label {label}");
                yield break;
            }

            IList<ShapeSO> list = handle.Result;
            foreach (var item in list) {
                bool succ = shapes.TryAdd(item.shapeType, item);
                if (!succ) {
                    Debug.LogError($"Failed to add SquareSO with shapeType {item.shapeType} to dictionary");
                }
                Debug.Log($"Loaded SquareSO with shapeType {item.shapeType}");
            }

            shapeHandle = handle;
        }

        public bool Shape_TryGet(ShapeType shapeType, out ShapeSO shapeSO) {
            return shapes.TryGetValue(shapeType, out shapeSO);
        }
        #endregion

        #region SquareEntityPrefab
        IEnumerator SquareEntityPrefab_Load() {
            string address = "Entity_Square_Blue";
            var handle = Addressables.LoadAssetAsync<SquareEntity>(address);
            yield return handle;
            if (!handle.IsDone) {
                Debug.LogError($"Failed to load SquareEntity prefab with address {address}");
                yield break;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded) {
                Debug.LogError($"Failed to load SquareEntity prefab with address {address}");
                yield break;
            }

            squareEntityPrefab = handle.Result;
            squareEntityPrefabHandle = handle;
        }

        public SquareEntity GetSquareEntityPrefab() {
            if (squareEntityPrefab == null) {
                Debug.LogError("SquareEntity prefab is not loaded");
            }
            return squareEntityPrefab;
        }
        #endregion
    }
}