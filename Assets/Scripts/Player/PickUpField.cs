using UnityEngine;
using UnityEngine.Events;

public class PickUpField : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private PlayerInventory _playerInventory;

    public UnityAction<Item> ItemInPickUpField;
    public UnityAction<Item> ItemOutPickUpField;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Item item))
            ItemInPickUpField?.Invoke(item);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out Item item))
            ItemOutPickUpField?.Invoke(item);
    }
}
