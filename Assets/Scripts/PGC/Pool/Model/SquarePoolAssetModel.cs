using System;
using PGC.Enum;
using UnityEngine;

namespace PGC.Pool.Model
{
    [Serializable]
    public class SquarePoolAssetModel
    {
        public GameObject prefab;
        public ItemAbilityType abilityType;
        public SquareColorEnum squareColor;
        public int size;
    }
}