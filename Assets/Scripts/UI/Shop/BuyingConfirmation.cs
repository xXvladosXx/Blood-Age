using System;
using InventorySystem.Items;
using ShopSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class BuyingConfirmation : TransactionConfirmation
    {
        protected override void ConfirmTransaction()
        {
            if (Customer.GetGold() < ItemToPurchase.Price * CurrentAmount)
            {
                RejectTransaction();
                return;
            }

            Seller.ConfirmSelling(ItemToPurchase, Customer, CurrentAmount);
            base.ConfirmTransaction();
        }

        protected override void TextTransaction()
        {
            _amount.text = CurrentAmount.ToString();
            var startPrice = ItemToPurchase.Price * CurrentAmount;
            startPrice += (startPrice * Seller.GetPriceModifier) / 100;
            
            Yes.interactable = Customer.GetGold() >= startPrice;
            
            _confirmationText.text = $"Are you sure you want to buy {ItemToPurchase.name}?\n " +
                                     $"Price: {startPrice}";

        }
    }
}
