namespace UI.Shop
{
    public class SellingConfirmation : TransactionConfirmation
    {
        protected override void ConfirmTransaction()
        {
            Customer.ConfirmSelling(ItemToPurchase, Seller, CurrentAmount);
            base.ConfirmTransaction();
        }

        protected override void TextTransaction()
        {
            _amount.text = CurrentAmount.ToString();

            _confirmationText.text = $"Are you sure you want to sell {ItemToPurchase.name}?\n " +
                                     $"Price: {ItemToPurchase.Price*CurrentAmount}";
        }
    }
}