using InventorySystem.Items;
using UnityEngine;

namespace QuestSystem.QuestObjectives
{
    [CreateAssetMenu (menuName = "Quests/Objectives/ReceivingObjective")]

    public class ReceivingObjective : Objective
    {
        [SerializeField] private InventoryItem _inventoryItem;
        
        public InventoryItem GetItem => _inventoryItem;
    }
}