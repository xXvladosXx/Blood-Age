using ShopSystem;
using UI.Shop;
using UnityEngine;

namespace DialogueSystem.AIDialogue
{
    [CreateAssetMenu (menuName = "DialogueSystem/Channels/AIShopChanel")]

    public class AIShopChanel : AIDialogueChanel
    {
        public override void Visit(AIConversant aiConversant, PlayerConversant playerConversant)
        {
            var seller = aiConversant.GetComponent<Seller>();
            SellerPanel.Instance.ShowSellerInventory(seller.GetInventoryContainer, seller);
            CustomerPanel.Instance.SetSeller(seller);
        }
    }
}