using InventorySystem;
using UI.Inventory;
using UnityEngine;

namespace UI.Tooltip
{
    public class MouseData
    {
        public UserInterface UI { get; set; }
        public GameObject TempItemDrag { get; set; }
        public GameObject TempItemHover { get; set; }
        public GameObject LastItemClicked { get; set; }
    }
}