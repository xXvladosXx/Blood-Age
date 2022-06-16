using System;
using InventorySystem;
using InventorySystem.Items;
using ShopSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public abstract class TransactionConfirmation : MonoBehaviour
    {
        [SerializeField] protected Button Yes;
        [SerializeField] private Button _no;
        [SerializeField] private Button _add;
        [SerializeField] private Button _remove;
        [SerializeField] protected TextMeshProUGUI _amount;
        [SerializeField] protected TextMeshProUGUI _confirmationText;

        protected Customer Customer;
        protected InventoryItem ItemToPurchase;
        protected Seller Seller;
        protected int MaxAmount;
        protected int CurrentAmount = 1;

        private void OnEnable()
        {
            Yes.onClick.AddListener(ConfirmTransaction);
            _no.onClick.AddListener(RejectTransaction);
            _add.onClick.AddListener(AddAmount);
            _remove.onClick.AddListener(RemoveAmount);
        }

        private void Update()
        {
            _add.interactable = CurrentAmount < MaxAmount;
            _remove.interactable = CurrentAmount > 1;
        }

        private void AddAmount()
        {
            CurrentAmount++;
            TextTransaction();
            _amount.text = CurrentAmount.ToString();
        }

        private void RemoveAmount()
        {
            CurrentAmount--;
            TextTransaction();
            _amount.text = CurrentAmount.ToString();
        }

        protected virtual void ConfirmTransaction()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Yes.onClick.RemoveAllListeners();
            _no.onClick.RemoveAllListeners();
            _add.onClick.RemoveAllListeners();
            _remove.onClick.RemoveAllListeners();
        }
        
        protected void RejectTransaction()
        {
            gameObject.SetActive(false);
        }

        public void SetItemToPurchase(InventoryItem inventoryItem, Customer customer, Seller seller, int amount = 1)
        {
            Customer = customer;
            ItemToPurchase = inventoryItem;
            Seller = seller;
            MaxAmount = amount;
            CurrentAmount = 1;
            TextTransaction();
        }

        protected abstract void TextTransaction();
    }
}