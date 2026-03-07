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

        public AssetModule() {
            shapes = new Dictionary<ShapeType, ShapeSO>();
        }

        public IEnumerator LoadAllIE() {
            yield return Shape_Load();
        }

        public void UnloadAll() {
            if (shapeHandle.IsValid()) {
                Addressables.Release(shapeHandle);
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
    }
}