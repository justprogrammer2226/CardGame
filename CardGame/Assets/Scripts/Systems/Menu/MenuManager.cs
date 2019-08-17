using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _settingsMenu;

    [Header("Debug")]
    [SerializeField] private GameObject _lastActiveMenu;

    private void Start()
    {
        _startMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _lastActiveMenu = _startMenu;
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
}
