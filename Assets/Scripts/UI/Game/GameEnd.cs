using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class GameEnd : MonoBehaviour
{
    private const int MenuSceneIndex = 0;
    private const int GameSceneIndex = 1;
    private const string GameEndMassage = "Victory";
    private const string PlayerDiedMassage = "You Died";

    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _bossDefeatedText;
    [SerializeField] private Button _menu;
    [SerializeField] private Button _restart;
    [SerializeField] private Image _gameEndPanel;
    [SerializeField] private Color _victoryColor;
    [SerializeField] private Color _defeatColor;
    [SerializeField] private RectTransform _interface;
    [SerializeField] private WaveGenerator _waveGenerator;
    [SerializeField] private Player _player;

    private CanvasGroup _canvasGroup;
    private bool _isGameEnd = false;

    public bool IsGameEnd => _isGameEnd;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        _menu.onClick.AddListener(OnMenuButtonClick);
        _restart.onClick.AddListener(OnRestartButtonClick);       
    }

    private void Start()
    {
        _player.Health.Died += OnPlayerDied;
        _waveGenerator.GameEnded += OnGameEnded;
    }

    private void OnGameEnded() => StartCoroutine(Victory(GameEndMassage));

    private void OnPlayerDied() => SetActivity(PlayerDiedMassage, false);

    private void SetActivity(string massage, bool isVictory)
    {
        if (isVictory)
            _gameEndPanel.color = _victoryColor;
        else
            _gameEndPanel.color = _defeatColor;

        _text.text = massage;
        _isGameEnd = true;
        _player.GameEnd();
        _interface.gameObject.SetActive(false);
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator Victory(string massage)
    {
        WaitForSeconds delay = new WaitForSeconds(7);

        _bossDefeatedText.gameObject.SetActive(true);

        yield return delay;
        _bossDefeatedText.gameObject.SetActive(false);
        SetActivity(massage, true);
    }

    private void OnDisable()
    {
        _menu.onClick.RemoveListener(OnMenuButtonClick);
        _restart.onClick.RemoveListener(OnRestartButtonClick);
    }

    private void OnRestartButtonClick() => SceneManager.LoadScene(GameSceneIndex);

    private void OnMenuButtonClick() => SceneManager.LoadScene(MenuSceneIndex);
}
