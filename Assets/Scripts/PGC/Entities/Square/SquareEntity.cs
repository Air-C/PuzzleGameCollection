using PGC.Enum;
using UnityEngine;

namespace Entities
{
    public class SquareEntity
    {

        private int score = 10;
        public int X { get; set; }
        public int Y { get; set; }
        public GameObject SquareObj {get; set; }
        private ItemAbilityType abilityType = ItemAbilityType.None;

        public ItemAbilityType AbilityType
        {
            get => abilityType;
            set => abilityType = value;
        }

        public int Score
        {
            get => score;
            set => score = value;
        }

        public SquareEntity(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void RestIndex(int x, int y)
        {
            X = x;
            Y = y;
        }
        
    }
}