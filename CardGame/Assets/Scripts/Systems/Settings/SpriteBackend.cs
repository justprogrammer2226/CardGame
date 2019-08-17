using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteBackend : MonoBehaviour, IPointerDownHandler
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        int indexCurrentImage = SettingsManager.instance.GetIndexByImage(_image);
        SettingsManager.instance.SelectSpriteBackend(indexCurrentImage);
    }
}
