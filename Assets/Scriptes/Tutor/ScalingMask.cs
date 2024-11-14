using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class ScalingMask : MonoBehaviour
{
    [SerializeField] private TypeScaling _type;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _delta;
    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private TextMeshProUGUI _textFinger;

    private FitMask _mask;
    private RectTransform _rect;
    private Image _image;
    private Tween _tween;

    private enum TypeScaling
    {
        Scaling,
        Move,
        None
    }

    private void Awake()
    {
        _mask = GetComponent<FitMask>();
        _rect = GetComponent<RectTransform>();
        TryGetComponent(out _image); 
    }

    private void OnDisable()
    {
        Debug.Log("disable");
        DisableText();
        _tween.Kill();
    }

    public void DisableText()
    {
        if(_textFinger != null)
            _textFinger.gameObject.SetActive(false);
    }

    private int _index = 0;

    public bool SetText(string text)
    {
        if (_textFinger != null)
        {
            _textFinger.gameObject.SetActive(true);
            _textFinger.text = text;
            return true; 
        }
        else 
            return false;
    }

    private void OnEnable()
    {
        if (_type == TypeScaling.None) return;
        _index = 0;
        _rect.localPosition = _startPosition;
        if (_image != null)
        {
            _mask.Sprite = _sprites[_index];
            _image.sprite = _sprites[_index];
        }
        switch (_type)
        {
            case TypeScaling.Scaling:
                _tween = _rect
                    .DOScale(.8f, _duration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Yoyo)
                    .OnStepComplete(() =>
                    {
                        if (_image != null)
                        {
                            _index = (_index + 1) % _sprites.Length;
                            _mask.Sprite = _sprites[_index];
                            _image.sprite = _sprites[_index];
                        }
                    }); 
                break;
            case TypeScaling.Move:
                _tween = _rect
                    .DOAnchorPos(_delta, _duration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Yoyo)
                    .OnStepComplete(() => { 
                        _index = (_index + 1) % _sprites.Length;
                        _mask.Sprite = _sprites[_index];
                        _image.sprite = _sprites[_index]; 
                    });
                break;
        }
        
    }
}
