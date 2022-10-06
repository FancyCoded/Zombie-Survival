using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(ItemView))]
public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Transform _prevParent;
    private InventoryView _inventoryView;
    private CanvasGroup _canvasGroup;
    private Item _item;
    private ItemBelong _itemBelong = ItemBelong.Ground;
    private int _itemCount;

    public ItemBelong ItemBelong => _itemBelong;
    public Item Item => _item;
    public int ItemCount => _itemCount;

    public UnityAction<Drag> ItemDropped;
    public UnityAction<int> ItemCountChanged;

    private void Start() =>
        _canvasGroup = GetComponent<CanvasGroup>();

    public void Initialize(InventoryView inventoryView, Item item, ItemBelong itemBelong, int itemCount = 1)
    {
        _inventoryView = inventoryView;
        _item = item;
        _itemBelong = itemBelong;
        _itemCount = itemCount;
    }

    public void ChangeCount(int itemCount)
    {
        _itemCount = itemCount;
        ItemCountChanged?.Invoke(_itemCount);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _prevParent = transform.parent;
        transform.SetParent(_inventoryView.transform);
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) =>
        transform.position = Input.mousePosition;

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        if (transform.parent == _inventoryView.transform)
            transform.SetParent(_prevParent);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && _itemBelong != ItemBelong.Ground)
            _inventoryView.DropItem(this);
        else if (eventData.button == PointerEventData.InputButton.Right && _itemBelong == ItemBelong.PlayerInventoty)
        {
            if(Item is Healer healer)
                _inventoryView.UseHealer(healer.Properties.Name);
        }
    }
}
