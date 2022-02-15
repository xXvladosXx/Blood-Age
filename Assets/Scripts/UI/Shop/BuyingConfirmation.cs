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

            Seller.ConfirmPurchase(ItemToPurchase, Customer, CurrentAmount);
            base.ConfirmTransaction();
        }

        protected override void TextTransaction()
        {
            _amount.text = CurrentAmount.ToString();

            _confirmationText.text = $"Are you sure you want to buy {ItemToPurchase.name}?\n " +
                                     $"Price: {ItemToPurchase.Price * CurrentAmount}";

        }
    }
}
