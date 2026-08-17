using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Keeps music playing between scenes and provides named methods for the
    /// small set of sound effects used by the game.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Music")]
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip gameMusic;

        [Header("Tower Sounds")]
        [SerializeField] private AudioClip machineGunSound;
        [SerializeField] private AudioClip laserSound;
        [SerializeField] private AudioClip rocketSound;

        [Header("Game Sounds")]
        [SerializeField] private AudioClip enemyDeathSound;
        [SerializeField] private AudioClip baseDamageSound;
        [SerializeField] private AudioClip buildSound;
        [SerializeField] private AudioClip upgradeSound;
        [SerializeField] private AudioClip sellSound;
        [SerializeField] private AudioClip victorySound;
        [SerializeField] private AudioClip defeatSound;
        [SerializeField] private AudioClip buttonSound;

        public float MusicVolume => musicSource.volume;
        public float SfxVolume => sfxSource.volume;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            EnsureAudioListener();
            DontDestroyOnLoad(gameObject);
            musicSource.volume = SaveSystem.Data.musicVolume;
            sfxSource.volume = SaveSystem.Data.sfxVolume;
        }

        private void OnEnable()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        private void Start()
        {
            PlayMusicForScene(SceneManager.GetActiveScene().name);
        }

        private void OnDisable()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EnsureAudioListener();
            PlayMusicForScene(scene.name);
        }

        private void EnsureAudioListener()
        {
            AudioListener[] listeners = FindObjectsByType<AudioListener>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);

            foreach (AudioListener listener in listeners)
            {
                if (listener.enabled)
                {
                    return;
                }
            }

            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("AudioManager could not find the Main Camera for audio playback.");
                return;
            }

            AudioListener cameraListener = mainCamera.GetComponent<AudioListener>();
            if (cameraListener == null)
            {
                cameraListener = mainCamera.gameObject.AddComponent<AudioListener>();
            }

            cameraListener.enabled = true;
        }

        private void PlayMusicForScene(string sceneName)
        {
            AudioClip nextMusic = sceneName == "MainMenu" ? menuMusic : gameMusic;
            if (musicSource.clip == nextMusic && musicSource.isPlaying)
            {
                return;
            }

            musicSource.clip = nextMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = Mathf.Clamp01(volume);
            SaveSystem.SetVolumes(musicSource.volume, sfxSource.volume);
        }

        public void SetSfxVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
            SaveSystem.SetVolumes(musicSource.volume, sfxSource.volume);
        }

        public void PlayTowerShot(TowerType towerType)
        {
            switch (towerType)
            {
                case TowerType.MachineGun:
                    Play(machineGunSound, 0.35f);
                    break;
                case TowerType.Laser:
                    Play(laserSound, 0.45f);
                    break;
                case TowerType.Rocket:
                    Play(rocketSound, 0.7f);
                    break;
            }
        }

        public void PlayEnemyDeath() => Play(enemyDeathSound, 0.65f);
        public void PlayBaseDamage() => Play(baseDamageSound, 0.8f);
        public void PlayBuild() => Play(buildSound, 0.7f);
        public void PlayUpgrade() => Play(upgradeSound, 0.7f);
        public void PlaySell() => Play(sellSound, 0.7f);
        public void PlayVictory() => Play(victorySound, 0.9f);
        public void PlayDefeat() => Play(defeatSound, 0.9f);
        public void PlayButton() => Play(buttonSound, 0.45f);

        private void Play(AudioClip clip, float volumeScale)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip, volumeScale);
            }
        }

        // Used only by the editor scene builder.
        public void Configure(
            AudioSource music, AudioSource effects,
            AudioClip menu, AudioClip game,
            AudioClip machineGun, AudioClip laser, AudioClip rocket,
            AudioClip enemyDeath, AudioClip baseDamage,
            AudioClip build, AudioClip upgrade, AudioClip sell,
            AudioClip victory, AudioClip defeat, AudioClip button)
        {
            musicSource = music;
            sfxSource = effects;
            menuMusic = menu;
            gameMusic = game;
            machineGunSound = machineGun;
            laserSound = laser;
            rocketSound = rocket;
            enemyDeathSound = enemyDeath;
            baseDamageSound = baseDamage;
            buildSound = build;
            upgradeSound = upgrade;
            sellSound = sell;
            victorySound = victory;
            defeatSound = defeat;
            buttonSound = button;
        }
    }
}
