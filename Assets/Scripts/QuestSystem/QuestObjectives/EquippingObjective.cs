using System;
using InventorySystem.Items;
using StatsSystem;
using UnityEngine;

namespace QuestSystem.QuestObjectives
{
    [CreateAssetMenu (menuName = "Quests/Objectives/EquippingObjective")]
    public class EquippingObjective : Objective
    {
        [SerializeField] private InventoryItem _inventoryItem;
        public InventoryItem GetItem => _inventoryItem;
        
    }
    
}