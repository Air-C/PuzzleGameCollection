using System;
using UnityEngine;
using Custom.Tool;

namespace PGC {

    // 要渲染的, 可继承MonoBehaviour
    public class SquareEntity : MonoBehaviour {

        public int id;
        public UnityEngine.Object SquareObj { get; set; }
        public Int2 GridIndex { get; set; }
        public string AssetName { get; set; }

        // 继承了 MonoBehaviour的无法调用构造函数, 只能用这个方法来初始化
        public void Ctor() {

        }

        public void TF_Pos_Set(Vector3 pos) {
            transform.position = pos;
        }

    }
}