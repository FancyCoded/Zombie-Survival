using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerIK))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public partial class PlayerInventory : MonoBehaviour
{
    private const string HoldingType = "HoldType";
    private const string IsHoldingWeapon = "HoldingWeapon";
    private const string Reloading = "Reloading";
    private const string Reloaded = "Reloaded";
    private const string NeedToggleWeapon = "ToggleWeapon";

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _targetLook;
    [SerializeField] private WeaponSpawner _weaponSpawner;
    [SerializeField] private AmmunitionSpawner _ammunitonSpawner;
    [SerializeField] private HealerSpawner _healerSpawner;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private PlayerProperties _playerProperties;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private PickUpField _pickUpField;
    [SerializeField] private AudioClip _selectWeaponSound;
    [SerializeField] private AudioClip _pickUpSound;
    [SerializeField] private AudioClip _dropSound;
    [SerializeField] private AudioClip _takeHealSound;
    [SerializeField] private List<Item> _itemsInPickUpField = new List<Item>();
    [SerializeField] private int _maxCapacity;

    private Player _player;
    private PlayerIK _playerIK;
    private PlayerInput _playerInput;
    private AudioSource _audioSource;
    private Animator _animator;

    private Weapon _currentWeapon;
    private Weapon _targetWeapon;
    private Weapon _mainWeapon;
    private Weapon _secondWeapon;
    private Weapon _meleeWeapon;

    private Dictionary<string, ItemCell> _items = new Dictionary<string, ItemCell>();

    public PickUpField PickUpField => _pickUpField;
    public Weapon MainWeapon => _mainWeapon;
    public Weapon SecondWeapon => _secondWeapon;
    public Weapon MeleeWeapon => _meleeWeapon;
    public Weapon CurrentWeapon => _currentWeapon;
    public int MaxCapacity => _maxCapacity;

    public UnityAction<List<Item>> UpdatePickUpField;

    public UnityAction<ItemProperties, WeaponIndex, ItemBelong> WeaponAdded;
    public UnityAction<WeaponIndex> WeaponDropped;
    public UnityAction<int> WeaponAmmunitionsCountChanged;
    public UnityAction<Weapon, int> WeaponChanged;

    public UnityAction<Item, int, ItemBelong> ItemAdded;
    public UnityAction<Item, int> ItemCountChanged;
    public UnityAction<Item> ItemDropped;
    public UnityAction<string, int> ItemUsed;
    public UnityAction OnItemTraded;

    public UnityAction<int> InventoryCapacityChanged;
    public UnityAction<ItemProperties> MoneyIncame;
    
    private void OnEnable()
    {
        _inventoryView.ItemDropped += OnItemDropped;
        _inventoryView.WeaponDropped += OnWeaponDropped;
        _inventoryView.HealerUsed += OnHealerUsed;
        _inventoryView.ItemSold += OnItemSold;
        _pickUpField.ItemInPickUpField += OnItemInPickUpField;
        _pickUpField.ItemOutPickUpField += OnItemOutPickUpField;
    }

    private void OnDisable()
    {
        _inventoryView.ItemDropped -= OnItemDropped;
        _inventoryView.WeaponDropped -= OnWeaponDropped;
        _inventoryView.HealerUsed -= OnHealerUsed;
        _pickUpField.ItemInPickUpField -= OnItemInPickUpField;
        _pickUpField.ItemOutPickUpField -= OnItemOutPickUpField;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
        _playerIK = GetComponent<PlayerIK>();
        _playerInput = GetComponent<PlayerInput>();

        UnarmPlayer();
    }

    public int GetInventoryCapacity()
    {
        int capacity = 0;

        foreach (var cell in _items.Values)
        {
            if (cell.Item is Ammunition ammunition)
                capacity += cell.ItemCount * ammunition.Properties.Weight;
            if (cell.Item is Healer healer)
                capacity += cell.ItemCount * healer.Properties.Weight;
        }

        return capacity;
    }

    public void DropItemToInventory(Item item)
    {
        if (item is Healer healer)
            AddHealer(healer);
        if (item is Ammunition ammunition)
            AddAmmunition(ammunition);

        _audioSource.PlayOneShot(_pickUpSound);
        InventoryCapacityChanged?.Invoke(GetInventoryCapacity());
        OnItemOutPickUpField(item);
    }

    public void TryAddItem(Item item, RaycastHit hit)
    {
        bool isEnoughCapacity = false;

        if (item is Healer healer)
            if (IsEnoughCapacity(healer.Properties.Weight, out isEnoughCapacity))
                AddHealer(healer);

        if (item is Ammunition ammunition)
            if (IsEnoughCapacity(ammunition.Properties.Weight, out isEnoughCapacity))
                AddAmmunition(ammunition);

        if (isEnoughCapacity)
        {
            _audioSource.PlayOneShot(_pickUpSound);
            InventoryCapacityChanged?.Invoke(GetInventoryCapacity());
            OnItemOutPickUpField(item);
            hit.collider.gameObject.SetActive(false);
        }
    }

    public void AddBoughtItem(Item item)
    {
        if (item is Weapon weapon)
            PickUpWeapon(weapon);
        else
            AddItem(item);

        OnItemTraded?.Invoke();
    }

    public bool IsEnoughCapacity(float weight, out bool isEnough)
    {
        isEnough = GetInventoryCapacity() + weight <= _maxCapacity;
        return isEnough;
    }

    private void OnItemSold(Item item, ItemProperties itemProperties)
    {
        if (item is Weapon weapon)
        {
            if (_currentWeapon)
            {
                if (weapon.Index == _currentWeapon.Index)
                {
                    _currentWeapon = null;
                    UnarmPlayer();
                }
            }

            switch (weapon.Index)
            {
                case WeaponIndex.MainWeapon:
                    PrepareAndSoldWeapon(_mainWeapon);
                    _mainWeapon = null;
                    break;
                case WeaponIndex.SecondWeapon:
                    PrepareAndSoldWeapon(_secondWeapon);
                    _secondWeapon = null;
                    break;
                default:
                    PrepareAndSoldWeapon(_meleeWeapon);
                    _meleeWeapon = null;
                    break;
            }
        }

        if (item is Healer healer)
            ReduceHealerCount(healer);
        if(item is Ammunition ammunition)
            ReduceAmmunitionCount(ammunition);

        _audioSource.PlayOneShot(_dropSound);
        InventoryCapacityChanged?.Invoke(GetInventoryCapacity());
        MoneyIncame?.Invoke(itemProperties);
        OnItemTraded?.Invoke();
    }

    private void PrepareAndSoldWeapon(Weapon weapon)
    {
        weapon.ToggleIsPicked();
        weapon.Rigidbody.isKinematic = false;
        weapon.Collider.isTrigger = false;
        weapon.Collider.enabled = true;
        weapon.gameObject.SetActive(false);
        weapon.transform.SetParent(_weaponSpawner.Container);

        _audioSource.PlayOneShot(_dropSound);

        WeaponDropped?.Invoke(weapon.Index);
    }

    private void AddItem(Item item)
    {
        if (item is Healer healer)
            AddHealer(healer);

        if (item is Ammunition ammunition)
            AddAmmunition(ammunition);

        InventoryCapacityChanged?.Invoke(GetInventoryCapacity());
        item.gameObject.SetActive(false);
        _audioSource.PlayOneShot(_pickUpSound);
    }

    private bool IsMoreOne(string name) =>_items[name].ItemCount > 1;

    private void OnItemDropped(Item item)
    {
        Vector3 position = transform.position + transform.up;

        switch (item)
        {
            case Healer healer:
                ReduceHealerCount(healer);
                _healerSpawner.SpawnHealer(healer, position);
                break;
            case Ammunition ammunition:
                ReduceAmmunitionCount(ammunition);
                _ammunitonSpawner.SpawnAmmunition(ammunition, position);
                break;
        }

        _audioSource.PlayOneShot(_dropSound);
        InventoryCapacityChanged?.Invoke(GetInventoryCapacity());
        UpdatePickUpField?.Invoke(_itemsInPickUpField);
    }

    private void OnItemOutPickUpField(Item item)
    {
        _itemsInPickUpField.Remove(item);
        UpdatePickUpField?.Invoke(_itemsInPickUpField);
    }

    private void OnItemInPickUpField(Item item)
    {
        _itemsInPickUpField.Add(item);
        UpdatePickUpField?.Invoke(_itemsInPickUpField);
    }
}