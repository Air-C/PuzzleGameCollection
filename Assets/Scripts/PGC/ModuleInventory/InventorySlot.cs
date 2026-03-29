namespace PGC.ModuleInventory
{
    public class InventorySlot
    {
        private ItemEntity slot;
        private int count = 1;

        public InventorySlot(ItemEntity slot)
        {
            this.slot = slot;
        }
        
        public ItemEntity Slot
        {
            get => slot;
            set => slot = value;
        }

        public int Count
        {
            get => count;
            set => count = value;
        }
    }
}