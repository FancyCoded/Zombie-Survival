using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class TraderView : MonoBehaviour, ITradingView
{
    private const string PlayerNotEnoughMoneyText = "Money not enough!";
    private const string PlayerOverloadedText = "Your inventory overloaded!";

    [SerializeField] private Transform _traderInterface;
    [SerializeField] private Transform _traderContent;
    [SerializeField] private Button _exit;
    [SerializeField] private TradingItemView _tradingItemView;
    [SerializeField] private Trader _trader;
    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private RectTransform _errorPanel;
    [SerializeField] private TMP_Text _errorMassage;
    [SerializeField] private float _errorMassageDelay = 1.5f;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private WaveGenerator _waveGenerator;
 
    private CanvasGroup _canvasGroup;

    private IEnumerator _playerMoneyNotEnoughError;
    private IEnumerator _playerOverloadedErrror;

    public UnityAction TradingEnded;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        SetActivity(false);
    }

    private void OnEnable()
    {
        _exit.onClick.AddListener(OnExitButtonClick);
        _trader.PlayerMoneyNotEnough += OnPlayerMoneyNotEnough;
        _trader.PlayerOverloaded += OnPlayerOverloaded;
        _trader.TraderClosed += OnTraderClosed;
        _waveGenerator.WaveDelayStarted += OnWaveDelayStarted;
    }

    private void OnDisable()
    {
        _exit.onClick.RemoveListener(OnExitButtonClick);
        _trader.PlayerMoneyNotEnough -= OnPlayerMoneyNotEnough;
        _trader.PlayerOverloaded -= OnPlayerOverloaded;
        _trader.TraderClosed -= OnTraderClosed;
        _waveGenerator.WaveDelayStarted -= OnWaveDelayStarted;
    }

    public void Initialize(ItemProperties[] itemsProperties)
    {
        foreach (var itemProperties in itemsProperties)
            AddCell(itemProperties.ItemTemplate, ItemBelong.Trader);
    }

    public void ShowStore()
    {
        SetActivity(true);
        _cameraHandler.SetActivity(false);
    }

    public void Sell(string name) => _trader.Sell(name);

    private void OnWaveDelayStarted(int seconds) => StartCoroutine(TimeBeforeClose(seconds));
        
    private IEnumerator TimeBeforeClose(int seconds)
    {
        WaitForSeconds second = new WaitForSeconds(1);

        for(int i = seconds; i >= 0; i--)
        {
            _timer.text = i.ToString();
            yield return second;
        }
    }

    private void OnPlayerOverloaded()
    {
        if (_playerOverloadedErrror != null)
            StopCoroutine(_playerOverloadedErrror);

        _playerOverloadedErrror = PlayerOverloadedError();
        StartCoroutine(_playerOverloadedErrror);
    }

    private void OnPlayerMoneyNotEnough()
    {
        if (_playerMoneyNotEnoughError != null)
            StopCoroutine(_playerMoneyNotEnoughError);

        _playerMoneyNotEnoughError = PlayerMoneyNotEnoughError();
        StartCoroutine(_playerMoneyNotEnoughError);
    }

    private void OnExitButtonClick()
    {
        SetActivity(false);
        _cameraHandler.SetActivity(true);
        TradingEnded?.Invoke();
    }

    private void OnTraderClosed(bool isTrading)
    {
        SetActivity(false);

        if(isTrading)
            _cameraHandler.SetActivity(true);
    }

    private void SetActivity(bool isActive = false)
    {
        if (isActive == false)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        else
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
    }

    private void AddCell(Item item, ItemBelong belong)
    {
        TradingItemView tradingItemView = Instantiate(_tradingItemView);

        if(item is MeleeWeapon meleeWeapon)
            tradingItemView.Initialize(meleeWeapon.Properties, belong, this);

        if (item is FireArm fireArm)
            tradingItemView.Initialize(fireArm.Properties, belong, this);

        if (item is Ammunition ammunition)
            tradingItemView.Initialize(ammunition.Properties, belong, this);

        if (item is Healer healer)
            tradingItemView.Initialize(healer.Properties, belong, this);

        tradingItemView.transform.SetParent(_traderContent);
        tradingItemView.transform.localScale = new Vector3(1, 1, 1);
    }

    private IEnumerator PlayerMoneyNotEnoughError()
    {
        WaitForSeconds delay = new WaitForSeconds(_errorMassageDelay);

        _errorPanel.gameObject.SetActive(true);
        _errorMassage.gameObject.SetActive(true);
        _errorMassage.text = PlayerNotEnoughMoneyText;

        yield return delay;
        _errorPanel.gameObject.SetActive(false);
        _errorMassage.gameObject.SetActive(false);
    }

    private IEnumerator PlayerOverloadedError()
    {
        WaitForSeconds delay = new WaitForSeconds(_errorMassageDelay);

        _errorPanel.gameObject.SetActive(true);
        _errorMassage.gameObject.SetActive(true);
        _errorMassage.text = PlayerOverloadedText;

        yield return delay;
        _errorPanel.gameObject.SetActive(false);
        _errorMassage.gameObject.SetActive(false);
    }
}
