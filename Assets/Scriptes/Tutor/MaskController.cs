using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskController : MonoBehaviour
{
    [SerializeField] private Image _maskImage;
    [SerializeField] private int _countIterations;
    [SerializeField] private List<string> _textsPart;
    [SerializeField] private int _indexShow = 0;

    [SerializeField] private bool _fadeMask = false;
    [SerializeField] private bool _interactablePanel = false;
    [SerializeField] private bool _moveCharacter = false;


    private int _currentIndex = 0;

    public bool ActiveMask => _maskImage.gameObject.activeSelf;

    public int CountIterations => _countIterations;

    public List<string> TextsPart => _textsPart;

    public bool FadeMask => _fadeMask; 

    private void ShowMask(bool isOn)
    {
        _maskImage.gameObject.SetActive(isOn);
    }

    public bool UpdateMask(TextMeshProUGUI textMask)
    {
        ShowMask(_currentIndex == _indexShow);
        if(_currentIndex < _textsPart.Count)
            textMask.text = _textsPart[_currentIndex];
        _currentIndex++;
        
        return _currentIndex == _textsPart.Count + 1;
    }

    public bool UpdateMask(TextMeshProUGUI textMask, Button button)
    {
        ShowMask(_currentIndex == _indexShow);
        if(_indexShow > 0)
            button.interactable = _currentIndex != _indexShow;
        else
            button.interactable = _interactablePanel;
        if (_currentIndex < _textsPart.Count)
            textMask.text = _textsPart[_currentIndex];
        _currentIndex++;

        return _currentIndex == _textsPart.Count + 1;
    }

    public bool UpdateMask(TextMeshProUGUI textMask, Button button, RectTransform character)
    {
        if(_moveCharacter)
            character.DOMoveY(0, .1f);
        return UpdateMask(textMask, button);
    }
}
