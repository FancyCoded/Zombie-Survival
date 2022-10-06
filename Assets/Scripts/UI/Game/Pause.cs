using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private const int MainMenuSceneIndex = 0;

    [SerializeField] private RectTransform _pausePanel;
    [SerializeField] private Settings _settingsMenu;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _mainMenu;
    [SerializeField] private Button _resume;
    [SerializeField] private MusicTracker _musicTracker;

    private bool _isActive;

    public bool IsActive => _isActive;

    private void OnEnable()
    {
        _settings.onClick.AddListener(OnSettingsButtonClicked);
        _mainMenu.onClick.AddListener(OnMenuButtonClicked);
        _resume.onClick.AddListener(ToggleActivityPauseMenu);
        _settingsMenu.SettingsExitButtonClicked += OnSettingsExitButtonClicked;
    }

    private void OnDisable()
    {
        _settings.onClick.RemoveListener(OnSettingsButtonClicked);
        _mainMenu.onClick.RemoveListener(OnMenuButtonClicked);
        _resume.onClick.RemoveListener(ToggleActivityPauseMenu);
        _settingsMenu.SettingsExitButtonClicked -= OnSettingsExitButtonClicked;
    }

    public void ToggleActivityPauseMenu()
    {
        if (_pausePanel.gameObject.activeSelf == false)
        {
            SetActivityPauseMenu(true);
            Time.timeScale = 0;
            _musicTracker.AudioSource.Pause();
        }
        else
        {
            SetActivityPauseMenu(false);
            Time.timeScale = 1;
            _musicTracker.AudioSource.UnPause();
        }
    }

    public bool IsPauseActive() => 
        _pausePanel.gameObject.activeSelf || _settingsMenu.gameObject.activeSelf;
    
    public bool IsSettingActive() => _settingsMenu.gameObject.activeSelf;

    private void OnMenuButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(MainMenuSceneIndex);
    }

    private void OnSettingsButtonClicked()
    {
        _settingsMenu.SetActivity(true);
        SetActivityPauseMenu(false);
    }

    private void OnSettingsExitButtonClicked()
    {
        _settingsMenu.SetActivity(false);
        SetActivityPauseMenu(true);
    }

    private void SetActivityPauseMenu(bool isActive)
    {
        _pausePanel.gameObject.SetActive(isActive);
        _isActive = IsPauseActive();
    }
}
