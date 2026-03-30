using System;
using PGC.Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace PGC.ModuleInventory
{
    [Serializable]
    public class ItemEntity
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private string description;
        [SerializeField]
        private ItemAbilityType abilityType;
        [SerializeField]
        private GameObject prefab;

        public GameObject Prefab => prefab;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public ItemAbilityType AbilityType
        {
            get => abilityType;
            set => abilityType = value;
        }
    }
}