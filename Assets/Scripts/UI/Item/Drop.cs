using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PickUpField _boxCollider;

    private Drag _drag;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out Drag drag))
        {
            _drag = drag;

            if (_drag.Item is Weapon weapon && _drag.ItemBelong == ItemBelong.Ground)
                DropWeaponToCell(weapon);
            else if (_drag.ItemBelong == ItemBelong.Ground)
                if (IsInventoryDropPlace())
                    TryDropItemToInventory();
        }
    }

    private void DropWeaponToCell(Weapon weapon)
    {
        if (IsWeaponCells())
        {
            Destroy(_drag.gameObject);
            _playerInventory.PickUpWeapon(weapon);
        }
    }

    private void TryDropItemToInventory()
    {
        bool isEnoughCapacity = false;

        if (_drag.Item is Ammunition ammunition)
            _playerInventory.IsEnoughCapacity(ammunition.Properties.Weight, out isEnoughCapacity);
        if (_drag.Item is Healer healer)
            _playerInventory.IsEnoughCapacity(healer.Properties.Weight, out isEnoughCapacity);

        if (isEnoughCapacity)
            DropItem();
    }

    private void DropItem()
    {
        _drag.transform.SetParent(transform.GetChild(0));
        _drag.Item.gameObject.SetActive(false);
        _playerInventory.DropItemToInventory(_drag.Item);
        Destroy(_drag.gameObject);
    }

    private bool IsInventoryDropPlace() =>
        transform.name == _inventoryView.InventoryContent.parent.name;

    private bool IsWeaponCells() =>
        transform.name == _inventoryView.MainWeaponCell.name ||
        transform.name == _inventoryView.SecondWeaponCell.name ||
        transform.name == _inventoryView.MeleeWeaponCell.name;
}
