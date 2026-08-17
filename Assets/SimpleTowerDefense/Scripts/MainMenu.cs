using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SimpleTowerDefense
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Text savedProgressText;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject resetConfirmationPanel;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private void Start()
        {
            RefreshSavedProgress();
            settingsPanel.SetActive(false);
            resetConfirmationPanel.SetActive(false);
            mainPanel.SetActive(true);
        }

        public void Play()
        {
            AudioManager.Instance?.PlayButton();
            if (SceneFader.Instance != null)
            {
                SceneFader.Instance.LoadScene("Game");
            }
            else
            {
                SceneManager.LoadScene("Game");
            }
        }

        public void Quit()
        {
            AudioManager.Instance?.PlayButton();
            Application.Quit();
        }

        public void OpenSettings()
        {
            AudioManager.Instance?.PlayButton();
            musicSlider.SetValueWithoutNotify(SaveSystem.Data.musicVolume);
            sfxSlider.SetValueWithoutNotify(SaveSystem.Data.sfxVolume);
            mainPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            AudioManager.Instance?.PlayButton();
            settingsPanel.SetActive(false);
            mainPanel.SetActive(true);
        }

        public void SetMusicVolume(float volume)
        {
            AudioManager.Instance?.SetMusicVolume(volume);
        }

        public void SetSfxVolume(float volume)
        {
            AudioManager.Instance?.SetSfxVolume(volume);
        }

        public void AskToResetProgress()
        {
            AudioManager.Instance?.PlayButton();
            resetConfirmationPanel.SetActive(true);
        }

        public void CancelResetProgress()
        {
            AudioManager.Instance?.PlayButton();
            resetConfirmationPanel.SetActive(false);
        }

        public void ConfirmResetProgress()
        {
            AudioManager.Instance?.PlayButton();
            SaveSystem.ResetProgress();
            RefreshSavedProgress();
            float defaultMusicVolume = SaveSystem.Data.musicVolume;
            float defaultSfxVolume = SaveSystem.Data.sfxVolume;
            musicSlider.SetValueWithoutNotify(defaultMusicVolume);
            sfxSlider.SetValueWithoutNotify(defaultSfxVolume);
            AudioManager.Instance?.SetMusicVolume(defaultMusicVolume);
            AudioManager.Instance?.SetSfxVolume(defaultSfxVolume);
            resetConfirmationPanel.SetActive(false);
        }

        private void RefreshSavedProgress()
        {
            SaveData data = SaveSystem.Data;
            savedProgressText.text =
                $"High Score: {data.highScore}    Highest Wave: {data.highestWaveReached}";
        }

        // Used only when initially wiring the scene in the editor.
        public void Configure(
            Text progressText, GameObject menu, GameObject settings,
            GameObject resetConfirmation, Slider music, Slider effects)
        {
            savedProgressText = progressText;
            mainPanel = menu;
            settingsPanel = settings;
            resetConfirmationPanel = resetConfirmation;
            musicSlider = music;
            sfxSlider = effects;
        }
    }
}
