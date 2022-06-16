using ShopSystem;
using StateMachine;
using StateMachine.PlayerStates;
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
            var customer = playerConversant.GetComponent<Customer>();

            customer.OpenInventory(seller);
            
            SellerPanel.Instance.ShowSellerInventory(seller.GetInventoryContainer, seller);

            playerConversant.GetComponent<IStateSwitcher>().SwitchState<ShopPlayerState>().StartShopping(aiConversant, playerConversant);
        }
    }
}