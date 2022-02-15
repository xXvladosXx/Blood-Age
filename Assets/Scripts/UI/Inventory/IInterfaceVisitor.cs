namespace UI.Inventory
{
    using InventorySystem;

    public interface IInterfaceVisitor
    {
        void Visit(Item item, StaticInterface staticInterface);
        void Visit(Item item, DynamicInterface dynamicInterface);
    }
}