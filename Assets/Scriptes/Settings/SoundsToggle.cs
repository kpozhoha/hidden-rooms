using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SoundsToggle : MonoBehaviour
{
    public enum TypeSetting
    {
        Music,
        Sounds
    }

    [SerializeField] private TypeSetting typeSetting;
    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private Sprite[] _sprites;

    private Image _image;
    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnChange);
        _toggle.graphic.canvasRenderer.SetAlpha(1);
    }

    private void OnChange(bool isOn)
    {
        _image = _toggle.graphic as Image;
         _image.sprite = isOn ? _sprites[0] : _sprites[1];
        _toggle.graphic.canvasRenderer.SetAlpha(1);
        _audioMixer.audioMixer.SetFloat(typeSetting.ToString(), isOn ? 0f : -80f);
    }
}
