using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [SerializeField] private GameObject _backendImagesPrefab;
    [SerializeField] private GameObject _backendSpritesPanel;
    [SerializeField] private List<Sprite> _backendSprites;

    [Header("Debug")]
    [SerializeField] private int _idSelectedSprite;
    [SerializeField] private List<Image> _backendImages;

    private void Awake()
    {
        if(instance != null)
        {
            throw new System.Exception("On sceene must be one SettingsManager");
        }
        instance = this;
    }

    private void Start()
    {
        _idSelectedSprite = PlayerPrefs.GetInt("_idSelectedSprite", 0);

        if(SceneManager.GetActiveScene().name == "StartMenu")
        {
            SpawnImages();
            SelectSpriteBackend(_idSelectedSprite);
        }
    }

    private void SpawnImages()
    {
        _backendImages = new List<Image>();

        foreach(Sprite sprite in _backendSprites)
        {
            Image image = Instantiate(_backendImagesPrefab, _backendSpritesPanel.transform).GetComponent<Image>();
            image.sprite = sprite;
            _backendImages.Add(image);
        }
    }

    public void SelectSpriteBackend(int id)
    {
        foreach(Image image in _backendImages)
        {
            image.gameObject.transform.localScale = Vector3.one;
        }

        _idSelectedSprite = id;
        _backendImages[_idSelectedSprite].gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        PlayerPrefs.SetInt("_idSelectedSprite", _idSelectedSprite);
        PlayerPrefs.Save();
    }

    public int GetIndexByImage(Image image)
    {
        return _backendImages.IndexOf(image);
    }

    public Sprite GetBackendSprite()
    {
        return _backendSprites[_idSelectedSprite];
    }
}
