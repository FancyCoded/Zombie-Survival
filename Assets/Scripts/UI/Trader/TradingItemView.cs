using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradingItemView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _weight;
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private TMP_Text _tradingButtonText;
    [SerializeField] private Button _tradingButton;

    private TraderView _traderView;
    private InventoryView _inventoryView;
    private ItemProperties _itemProperites;
    private ItemBelong _itemBelong;

    private void OnEnable() =>
        _tradingButton.onClick.AddListener(OnTradingButtonClick);

    private void OnDisable() =>
        _tradingButton.onClick.RemoveListener(OnTradingButtonClick);

    public void Initialize(ItemProperties itemProperties, ItemBelong belong, ITradingView tradingView, int count = 1)
    {
        _itemProperites = itemProperties;
        _itemBelong = belong;
        _icon.sprite = itemProperties.Icon;
        _title.text = itemProperties.Name;
        _cost.text = itemProperties.Cost.ToString();

        if(tradingView is InventoryView inventoryView)
        {
            _count.text = count.ToString();
            _inventoryView = inventoryView;
            _tradingButtonText.text = "Sell";
        }
        if(tradingView is TraderView traderView)
        {
            _traderView = traderView;
            _tradingButtonText.text = "Buy";
        }

        if (_itemProperites.ItemTemplate is Weapon weapon)
            _weight.text = weapon.Index.ToString();
        else
            _weight.text = itemProperties.Weight.ToString();

    }

    private void OnTradingButtonClick()
    {
        if(_itemBelong == ItemBelong.Trader)
            _traderView.Sell(_itemProperites.Name);
        if (_itemBelong == ItemBelong.PlayerInventoty)
            _inventoryView.Sell(_itemProperites.ItemTemplate, _itemProperites);
    }
}