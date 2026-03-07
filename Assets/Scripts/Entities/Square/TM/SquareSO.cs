using System;
using UnityEngine;

namespace PGC {

    [CreateAssetMenu(fileName = "So_Square_", menuName = "PGC/SquareSO", order = 1)]
    public class SquareSO : ScriptableObject {

        public int typeID;

        public SquareEntity prefab;

    }

}