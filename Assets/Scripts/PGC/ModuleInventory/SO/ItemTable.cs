using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PGC.ModuleInventory.SO
{
    [CreateAssetMenu(menuName = "PGC/itemTable", fileName = "itemTable")]
    public class ItemTable : ScriptableObject
    {
        public List<ItemEntity> itemList = new ();
        
    }
}