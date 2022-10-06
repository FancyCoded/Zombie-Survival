using UnityEngine;

public partial class InventoryView : ITradingView
{
    public void TradingViewUpdate()
    {
        for (int i = 0; i < _tradingInventoryContent.childCount; i++)
            Destroy(_tradingInventoryContent.GetChild(i).gameObject);

        if(_items.Count > 0)
            foreach (var view in _items.Values)
                AddTradingCell(view.Drag.Item, view.Drag.ItemBelong, view.Drag.ItemCount);

        if (_playerInventory.MainWeapon)
            AddTradingCell(_playerInventory.MainWeapon, ItemBelong.PlayerInventoty);
        if (_playerInventory.SecondWeapon)
            AddTradingCell(_playerInventory.SecondWeapon, ItemBelong.PlayerInventoty);
        if (_playerInventory.MeleeWeapon)
            AddTradingCell(_playerInventory.MeleeWeapon, ItemBelong.PlayerInventoty);
    }

    public void Sell(Item item, ItemProperties itemProperties) =>
        ItemSold?.Invoke(item, itemProperties);

    private void AddTradingCell(Item item, ItemBelong belong, int count = 1)
    {
        TradingItemView tradingItemView = Instantiate(_tradingItemView);

        if (item is MeleeWeapon meleeWeapon)
            tradingItemView.Initialize(meleeWeapon.Properties, belong, this, count);

        if (item is FireArm fireArm)
            tradingItemView.Initialize(fireArm.Properties, belong, this, count);

        if (item is Ammunition ammunition)
            tradingItemView.Initialize(ammunition.Properties, belong, this, count);

        if (item is Healer healer)
            tradingItemView.Initialize(healer.Properties, belong, this, count);

        tradingItemView.transform.SetParent(_tradingInventoryContent);
        tradingItemView.transform.localScale = new Vector3(1, 1, 1);
    }
}
