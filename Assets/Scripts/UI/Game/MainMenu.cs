using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private const int GameSceneIndex = 1;

    [SerializeField] private Settings _settingsMenu;
    [SerializeField] private Button _start;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _exit;

    private BaseEventData ButtonSelected;

    private void OnEnable()
    {
        _start.onClick.AddListener(OnStartButtonClicked);
        _settings.onClick.AddListener(OnOptionsButtonClicked);
        _exit.onClick.AddListener(OnExitButtonClicked);
        _exit.OnSelect(ButtonSelected);
        _settingsMenu.SettingsExitButtonClicked += OnSettingsExitButtonClicked;
    }
    
    private void OnDisable()
    {
        _start.onClick.RemoveListener(OnStartButtonClicked);
        _settings.onClick.RemoveListener(OnOptionsButtonClicked);
        _exit.onClick.RemoveListener(OnExitButtonClicked);
        _settingsMenu.SettingsExitButtonClicked -= OnSettingsExitButtonClicked;
    }

    private void OnSettingsExitButtonClicked() => _settingsMenu.SetActivity(false);

    private void OnStartButtonClicked() => SceneManager.LoadScene(GameSceneIndex);

    private void OnOptionsButtonClicked() => _settingsMenu.gameObject.SetActive(true);

    private void OnExitButtonClicked() => Application.Quit();
}
