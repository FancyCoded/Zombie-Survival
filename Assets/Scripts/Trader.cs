using System;
using UnityEngine;
using UnityEngine.Events;

public class Trader : MonoBehaviour
{
    [SerializeField] private TraderStorage _traderStorage;
    [SerializeField] private TraderView _traderView;
    [SerializeField] private WeaponSpawner _weaponSpawner;
    [SerializeField] private WaveGenerator _waveGenerator;

    private bool _isOpen;
    private Player _player;

    public bool IsOpen => _isOpen;

    public UnityAction PlayerOverloaded;
    public UnityAction PlayerMoneyNotEnough;
    public UnityAction<bool> TraderClosed;

    private void Awake()
    {
        _traderView.Initialize(_traderStorage.Items);
        _isOpen = true;
    }

    private void OnEnable()
    {
        _waveGenerator.WaveStarted += OnWaveStarted;
        _waveGenerator.WaveEnded += OnWaveEnded;
        _traderView.TradingEnded += OnTradingEnded;
    }

    private void OnDisable()
    {
        _waveGenerator.WaveStarted -= OnWaveStarted;
        _waveGenerator.WaveEnded -= OnWaveEnded;
        _traderView.TradingEnded += OnTradingEnded;
    }

    public void Open(Player player)
    {
        _player = player;
        _traderView.ShowStore();
    }

    public void Sell(string name)
    {
        ItemProperties itemProperties;

        for(int i = 0; i < _traderStorage.Items.Length; i++)
        {
            itemProperties = _traderStorage.Items[i];

            if (IsContain(name, i))
            {
                if (IsEnoughPlayerMoney(i))
                {
                    if (isEnoughPlayerInventoyCapacity(itemProperties))
                    {
                        _player.Wallet.SpendMoney(itemProperties);

                        if(itemProperties.ItemTemplate is Weapon weapon)
                            _player.Buy(_weaponSpawner.SpawnAndGetWeapon(weapon));
                        if (itemProperties.ItemTemplate is Healer)
                            _player.Buy(itemProperties.ItemTemplate);
                        if (itemProperties.ItemTemplate is Ammunition)
                            _player.Buy(itemProperties.ItemTemplate);
                    }
                    else
                        PlayerOverloaded?.Invoke();
                }
                else
                    PlayerMoneyNotEnough?.Invoke();
            }
        }
    }

    private void OnTradingEnded() => _player = null;

    private void OnWaveEnded() => _isOpen = true;

    private void OnWaveStarted()
    {
        _isOpen = false;
        TraderClosed?.Invoke(_player != null);
        _player = null;
    }

    private bool isEnoughPlayerInventoyCapacity(ItemProperties itemProperties) =>
        _player.Inventory.IsEnoughCapacity(itemProperties.Weight, out bool isEnough);

    private bool IsContain(string name, int itemIndex) =>
        _traderStorage.Items[itemIndex].Name == name;

    private bool IsEnoughPlayerMoney(int itemIndex) =>
        _player.Wallet.Money >= _traderStorage.Items[itemIndex].Cost;
}