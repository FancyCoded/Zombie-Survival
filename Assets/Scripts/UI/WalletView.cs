using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _money;
    [SerializeField] private Wallet _wallet;

    private void OnEnable() => _wallet.MoneyChanged += OnMoneyChanged; 

    private void OnDisable() => _wallet.MoneyChanged -= OnMoneyChanged;

    private void OnMoneyChanged(int money)
    {
        _money.text = money.ToString();
    }
}
