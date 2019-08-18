using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _pauseMenu;

    [Header("Debug")]
    [SerializeField] private GameObject _lastActiveMenu;

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            _startMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            _lastActiveMenu = _startMenu;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _gameMenu.SetActive(true);
            _pauseMenu.SetActive(false);
            _lastActiveMenu = _gameMenu;
        }
    }

    public void ActivateStartMenu()
    {
        _startMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _startMenu;
    }

    public void ActivateSettingsMenu()
    {
        _settingsMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _settingsMenu;
    }

    public void ActivateGameMenu()
    {
        _gameMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _gameMenu;
    }

    public void ActivatePauseMenu()
    {
        _pauseMenu.SetActive(true);
        _lastActiveMenu.SetActive(false);
        _lastActiveMenu = _pauseMenu;
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
