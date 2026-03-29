using System.Collections.Generic;
using PGC.Pool.Model;
using UnityEngine;

namespace PGC.Pool.SO
{
    [CreateAssetMenu(menuName = "PGC/SquarePoolSettings" , fileName = "SquarePoolSettings")]
    public class SquarePoolSettings : ScriptableObject
    {
        public GameObject poolManager;
        public List<SquarePoolAssetModel> poolSettings = new ();
    }
}