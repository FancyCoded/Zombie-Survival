using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInventory))]
public class Wallet : MonoBehaviour
{
    [SerializeField] private int _money = 100;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private WaveGenerator _waveGenerator;

    private PlayerInventory _playerInventory;

    public int Money => _money;

    public UnityAction<int> MoneyChanged;

    private void Awake() => _playerInventory = GetComponent<PlayerInventory>();

    private void OnEnable() =>
        _playerInventory.MoneyIncame += OnMoneyIncome;

    private void Start() => MoneyChanged?.Invoke(_money);

    public void OnRewarded(int reward)
    {
        _money += reward;
        MoneyChanged?.Invoke(_money);
    }

    public void SpendMoney(ItemProperties itemProperties)
    {
        _money = Mathf.Clamp(_money - itemProperties.Cost, 0, int.MaxValue);
        MoneyChanged?.Invoke(_money);
    }

    private void OnMoneyIncome(ItemProperties itemProperties)
    {
        _money += itemProperties.Cost;
        MoneyChanged?.Invoke(_money);
    }
}