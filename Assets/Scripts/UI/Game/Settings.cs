using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Settings : MonoBehaviour
{
    private const string Resolution = "Resolution";
    private const string Quality = "Quality";
    private const string Fullscreen = "Fullscreen";
    private const int HighQualityIndex = 4;

    [SerializeField] private Button _exit;
    [SerializeField] private Button _saveSettings;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] private Toggle _fullScreenToggle;

    private Resolution[] _resolutions;
    private int _currentResolutionIndex = 0;

    public UnityAction SettingsExitButtonClicked;

    private void OnEnable()
    {
        _exit.onClick.AddListener(OnExitButtonClicked);
        _saveSettings.onClick.AddListener(OnSettingsSaved);
        _fullScreenToggle.onValueChanged.AddListener(OnFullScreenToggleChanged);
        _resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        _qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }

    private void OnDisable()
    {
        _exit.onClick.RemoveListener(OnExitButtonClicked);
        _saveSettings.onClick.RemoveListener(OnSettingsSaved);
        _fullScreenToggle.onValueChanged.RemoveListener(OnFullScreenToggleChanged);
        _resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
        _qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);
    }

    private void Start()
    {
        _resolutions = Screen.resolutions;
        SetDropdownOptions(_resolutionDropdown, GetResolutionsAsString());

        SetDropdownOptions(_qualityDropdown, GetQualities());

        LoadSettings(_currentResolutionIndex);
    }

    public void SetActivity(bool isActive) => gameObject.SetActive(isActive);

    private void OnQualityChanged(int index) => QualitySettings.SetQualityLevel(index);

    private void OnResolutionChanged(int index) =>
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreen);

    private void SetDropdownOptions(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.RefreshShownValue();
    }

    private void OnFullScreenToggleChanged(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    private void OnSettingsSaved()
    {
        PlayerPrefs.SetInt(Resolution, _resolutionDropdown.value);
        PlayerPrefs.SetInt(Quality, _qualityDropdown.value);
        PlayerPrefs.SetInt(Fullscreen, Convert.ToInt32(Screen.fullScreen));
    }

    private void LoadSettings(int resolutionIndex)
    {
        if (PlayerPrefs.HasKey(Resolution))
            _resolutionDropdown.value = PlayerPrefs.GetInt(Resolution);
        else
            _resolutionDropdown.value = resolutionIndex;

        if (PlayerPrefs.HasKey(Quality))
            _qualityDropdown.value = PlayerPrefs.GetInt(Quality);
        else
            _qualityDropdown.value = HighQualityIndex;

        if (PlayerPrefs.HasKey(Resolution))
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt(Fullscreen));
        else
            Screen.fullScreen = true;
    }

    private List<string> GetResolutionsAsString()
    {
        List<string> resolutions = new List<string>();
        string option;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            option = $"{_resolutions[i].width} x {_resolutions[i].height}";
            resolutions.Add(option);

            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                _currentResolutionIndex = i;
        }

        return resolutions;
    }

    private List<string> GetQualities()
    {
        List<string> _qualities = new List<string>();

        _qualities.AddRange(QualitySettings.names);

        return _qualities;
    }

    private void OnExitButtonClicked() => SettingsExitButtonClicked?.Invoke();
}
