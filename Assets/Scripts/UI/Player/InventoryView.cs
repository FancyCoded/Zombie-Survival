using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public partial class InventoryView : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private CameraHandler _cameraHandler;

    [Header("Inventory")]
    [SerializeField] private Transform _inventoryContent;
    [SerializeField] private Transform _groundContent;
    [SerializeField] private TMP_Text _inventoryCurrentWeight;
    [SerializeField] private TMP_Text _inventoryMaxWeight;
    [SerializeField] private ItemView _itemView;
    [SerializeField] private ItemView _weaponView;

    [Header("Trading Inventory")]
    [SerializeField] private Transform _tradingInventoryContent;
    [SerializeField] private TMP_Text _tradingInventoryCurrentWeight;
    [SerializeField] private TMP_Text _tradingInventoryMaxWeight;
    [SerializeField] private TradingItemView _tradingItemView;
    [SerializeField] private CanvasGroup _traderView;

    [Header("Weapon")]
    [SerializeField] private Transform _mainWeaponCell;
    [SerializeField] private Transform _secondWeaponCell;
    [SerializeField] private Transform _meleeWeaponCell;

    private CanvasGroup _canvasGroup;
    private Dictionary<string, ItemView> _items = new Dictionary<string, ItemView>();

    public Transform InventoryContent => _inventoryContent;
    public Transform MainWeaponCell => _mainWeaponCell;
    public Transform SecondWeaponCell => _secondWeaponCell;
    public Transform MeleeWeaponCell => _meleeWeaponCell;

    public UnityAction<Item> ItemDropped;
    public UnityAction<WeaponIndex> WeaponDropped;
    public UnityAction<string> HealerUsed;
    public UnityAction<Item, ItemProperties> ItemSold;

    private void OnEnable()
    {
        _playerInventory.UpdatePickUpField += OnUpdatePickUpField;
        _playerInventory.WeaponAdded += OnWeaponAdded;
        _playerInventory.WeaponDropped += OnWeaponDropped;
        _playerInventory.ItemAdded += OnItemAdded;
        _playerInventory.ItemCountChanged += OnItemCountChanged;
        _playerInventory.ItemDropped += OnItemDropped;
        _playerInventory.ItemUsed += OnItemUsed;
        _playerInventory.InventoryCapacityChanged += OnInventoryCapacityChanged;
        _playerInventory.OnItemTraded += TradingViewUpdate;
    }

    private void OnDisable()
    {
        _playerInventory.UpdatePickUpField -= OnUpdatePickUpField;
        _playerInventory.WeaponAdded -= OnWeaponAdded;
        _playerInventory.WeaponDropped -= OnWeaponDropped;
        _playerInventory.ItemAdded -= OnItemAdded;
        _playerInventory.ItemCountChanged -= OnItemCountChanged;
        _playerInventory.ItemDropped -= OnItemDropped;
        _playerInventory.ItemUsed -= OnItemUsed;
        _playerInventory.InventoryCapacityChanged -= OnInventoryCapacityChanged;
        _playerInventory.OnItemTraded -= TradingViewUpdate;
    }

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        SetCanvasGroup(0, false, false);
        _inventoryMaxWeight.text = _playerInventory.MaxCapacity.ToString();
        _tradingInventoryMaxWeight.text = _playerInventory.MaxCapacity.ToString();
    }

    public bool IsInteractable() => _canvasGroup.interactable || _traderView.interactable;

    public bool IsTrading() => _traderView.interactable;

    public void ToggleActivity()
    {
        if (_traderView.gameObject.activeSelf && _traderView.interactable)
            return;

        if (_canvasGroup.interactable == false)
            SetCanvasGroup(1, true, true);
        else
            SetCanvasGroup(0, false, false);

        _cameraHandler.ToggleAcitvity();
    }

    public void OnUpdatePickUpField(List<Item> items)
    {
        for (int i = 0; i < _groundContent.childCount; i++)
            Destroy(_groundContent.GetChild(i).gameObject);

        if (items.Count > 0)
            for (int i = 0; i < items.Count; i++)
                AddCell(items[i], 1, ItemBelong.Ground);
    }
    
    public void DropItem(Drag drag)
    {
        if (drag.Item is Weapon weapon)
            WeaponDropped?.Invoke(weapon.Index);
        else
            ItemDropped?.Invoke(drag.Item);
    }

    public void UseHealer(string name) => HealerUsed?.Invoke(name);

    private void OnItemUsed(string name, int itemCount)
    {
        if (itemCount == 0)
        {
            Destroy(_items[name].Drag.gameObject);
            _items.Remove(name);
        }
        else
            _items[name].Drag.ChangeCount(itemCount);
    }

    private void OnInventoryCapacityChanged(int capacity)
    {
        _inventoryCurrentWeight.text = capacity.ToString();
        _tradingInventoryCurrentWeight.text = capacity.ToString();
    }

    private void OnItemDropped(Item item)
    {
        if (item is Ammunition ammunition)
        {
            Destroy(_items[ammunition.Properties.Name].Drag.gameObject);
            _items.Remove(ammunition.Properties.Name);
        }
        if (item is Healer healer)
        {
            Destroy(_items[healer.Properties.Name].Drag.gameObject);
            _items.Remove(healer.Properties.Name);
        }
    }

    private void OnItemCountChanged(Item item, int itemCount)
    {
        if (item is Ammunition ammunition)
            _items[ammunition.Properties.Name].Drag.ChangeCount(itemCount);
        if (item is Healer healer)
            _items[healer.Properties.Name].Drag.ChangeCount(itemCount);
    }

    private void OnItemAdded(Item item, int itemCount, ItemBelong belong) =>
        AddCell(item, itemCount, belong);

    private void SetCanvasGroup(float alpha, bool isInteractable, bool blockRaycast)
    {
        _canvasGroup.alpha = alpha;
        _canvasGroup.interactable = isInteractable;
        _canvasGroup.blocksRaycasts = blockRaycast;
    }

    private void OnWeaponDropped(WeaponIndex index)
    {
        if (index == WeaponIndex.MainWeapon)
            Destroy(_mainWeaponCell.GetChild(0).gameObject);
        if (index == WeaponIndex.SecondWeapon)
            Destroy(_secondWeaponCell.GetChild(0).gameObject);
        if (index == WeaponIndex.MeleeWeapon)
            Destroy(_meleeWeaponCell.GetChild(0).gameObject);
    }

    private void OnWeaponAdded(ItemProperties properties, WeaponIndex index, ItemBelong belong)
    {
        ItemView view = Instantiate(_weaponView);

        if (view.TryGetComponent(out Drag drag))
        {
            drag.Initialize(this, properties.ItemTemplate, belong);

            if (index == WeaponIndex.MainWeapon)
                view.transform.SetParent(_mainWeaponCell);
            if (index == WeaponIndex.SecondWeapon)
                view.transform.SetParent(_secondWeaponCell);
            if (index == WeaponIndex.MeleeWeapon)
                view.transform.SetParent(_meleeWeaponCell);

            view.transform.localScale = new Vector3(1, 1, 1);
            view.Title.text = properties.Name;
            view.Image.sprite = properties.Icon;
        }
    }

    private void AddCell(Item item, int itemCount, ItemBelong itemBelong)
    {
        ItemView view;

        if (item is Weapon)
            view = Instantiate(_weaponView);
        else
            view = Instantiate(_itemView);

        if (item is FireArm fireArm)
            SetUpView(view, fireArm.Properties.Name, itemCount, fireArm.Properties.Icon);
        if (item is MeleeWeapon meleeWeapon)
            SetUpView(view, meleeWeapon.Properties.Name, itemCount, meleeWeapon.Properties.Icon);
        if (item is Healer healer)
            SetUpView(view, healer.Properties.Name, itemCount, healer.Properties.Icon);
        if (item is Ammunition ammunition)
            SetUpView(view, ammunition.Properties.Name, itemCount, ammunition.Properties.Icon);

        if (view.TryGetComponent(out Drag drag))
        {
            drag.Initialize(this, item, itemBelong, itemCount);

            if (drag.ItemBelong == ItemBelong.PlayerInventoty)
            {
                view.transform.SetParent(_inventoryContent);
                _items.Add(view.Title.text, view);
            }
            else
            {
                view.transform.SetParent(_groundContent);
            }
        }

        view.transform.localScale = new Vector3(1, 1, 1);
    }

    private void SetUpView(ItemView view, string name, int itemCount, Sprite sprite)
    {
        view.Title.text = name;
        view.Image.sprite = sprite;

        if(view.ItemCount)
            view.ItemCount.text = itemCount.ToString();
    }
}
