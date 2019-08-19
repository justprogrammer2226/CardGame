using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _endMenu;

    [Header("Buttons")]
    [SerializeField] private Button _retreatButton;
    [SerializeField] private Button _takeButton;

    [Header("Sliders")]
    [SerializeField] private Slider sfxSlider;

    [Header("Debug")]
    [SerializeField] private GameObject _lastActiveMenu;
    [SerializeField]  private float _lastSfxVolume;

    public void SetSfxVolume(float volume)
    {
        // This is done for optimization. Thus, we save fewer times.
        // We still will not hear the difference in 0.05. :)
        // So why save once again?
        if (Mathf.Abs(_lastSfxVolume - sfxSlider.value) > 0.05)
        {
            AudioManager.Instance.SetSfxVolume(volume);
            _lastSfxVolume = sfxSlider.value;
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            _startMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            _lastActiveMenu = _startMenu;
            if (PlayerPrefs.HasKey("sfxVolume"))
            {
                sfxSlider.value = AudioManager.Instance.GetSfxVolume();
            }

            sfxSlider.onValueChanged.AddListener((value) => SetSfxVolume(value));
            _lastSfxVolume = sfxSlider.value;

        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _gameMenu.SetActive(true);
            _pauseMenu.SetActive(false);
            _endMenu.SetActive(false);
            _lastActiveMenu = _gameMenu;
        }
    }

    public void ActivateStartMenu()
    {
        AudioManager.PlaySound("chipsCollide1");
        _startMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _startMenu;
    }

    public void ActivateSettingsMenu()
    {
        AudioManager.PlaySound("chipsCollide1");
        _settingsMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _settingsMenu;
    }

    public void ActivateGameMenu()
    {
        AudioManager.PlaySound("chipsCollide1");
        _gameMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _gameMenu;
    }

    public void ActivatePauseMenu()
    {
        AudioManager.PlaySound("chipsCollide1");
        _pauseMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _pauseMenu;
    }

    public void ActivateEndMenu()
    {
        AudioManager.PlaySound("chipsCollide1");
        _endMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _endMenu;
    }

    public void LoadStartMenu()
    {
        AudioManager.PlaySound("chipsCollide1");
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        AudioManager.PlaySound("chipsCollide1");
        SceneManager.LoadScene(1);
    }

    public void Retreat()
    {
        AudioManager.PlaySound("chipsCollide1");
        GameManager.instance.Retreat();
    }

    public void Take()
    {
        AudioManager.PlaySound("chipsCollide1");
        GameManager.instance.Take();
    }
}
