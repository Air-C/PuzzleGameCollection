using System.Collections.Generic;
using PGC.Enum;

namespace PGC.ModelEvent.Data
{
    public class AddItemEvent
    {
        public List<ItemAbilityType> items;

        public AddItemEvent(List<ItemAbilityType> items)
        {
            this.items = items;
        }
    }
}