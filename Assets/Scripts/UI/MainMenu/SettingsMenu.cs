using System;
using System.Collections.Generic;
using Entity;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class SettingsMenu : Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Toggle _fullScreen;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private TMP_Dropdown _quality;
        [SerializeField] private TMP_Dropdown _resolutionsDropdown;
        [SerializeField] private RenderPipelineAsset[] _qualityLevels;
        [SerializeField] private AudioMixer _audioMixer;

        private Resolution[] _resolutions;
        private int _screenInt;
        private bool _isFullScreen;
        
        private const string _qualityName = "optionvalue";
        private const string _resName = "resolutionoption";
        

        public override void Initialize()
        {
            SavingHandler = FindObjectOfType<SavingHandler>();

            _backButton.onClick.AddListener(MainMenuSwitcher.ShowLast);
            _screenInt = SavingHandler.GetIntFromPlayerPrefs("togglestate");
            
            if (_screenInt == 1)
            {
                _isFullScreen = true;
                _fullScreen.isOn = true;
            }
            else
            {
                _fullScreen.isOn = false;
            }
            
            int fullScreenIndex = SavingHandler.GetIntFromPlayerPrefs("togglestate");
            bool fullScreen = Convert.ToBoolean(fullScreenIndex);
            Screen.fullScreen = fullScreen; 
            
            _resolutionsDropdown.onValueChanged.AddListener(index =>
            {
                SavingHandler.SetIntToPlayerPrefs(_resName, _resolutionsDropdown.value);
            });
            _quality.onValueChanged.AddListener(index =>
            {
                SavingHandler.SetIntToPlayerPrefs(_qualityName, _quality.value);
                SetQuality(SavingHandler.GetIntFromPlayerPrefs(_qualityName));
            });
        }

        private void Start()
        {
            _volumeSlider.value = SavingHandler.GetFloatFromPlayerPrefs("NVolume");
            _audioMixer.SetFloat("Volume", _volumeSlider.value);

            _resolutions = Screen.resolutions;
            _resolutionsDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + "x" + _resolutions[i].height + " " +
                                _resolutions[i].refreshRate + "Hz";
                options.Add(option);
                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height &&
                    _resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
                {
                    currentResolutionIndex = i;
                }
            }
            
            _resolutionsDropdown.AddOptions(options);
            _resolutionsDropdown.value = PlayerPrefs.GetInt(_resName, currentResolutionIndex);
            _resolutionsDropdown.RefreshShownValue();

            _quality.value = PlayerPrefs.GetInt(_qualityName);
            QualitySettings.renderPipeline = _qualityLevels[_quality.value];
            QualitySettings.SetQualityLevel(_quality.value);
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        public void SetFullScreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            SavingHandler.SetIntToPlayerPrefs("togglestate", isFullscreen ? 1 : 0);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.renderPipeline = _qualityLevels[qualityIndex];
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetVolume(float volume)
        {
            SavingHandler.SetFloatToPlayerPrefs("NVolume", volume);
            _audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("NVolume"));
        }
    }
}