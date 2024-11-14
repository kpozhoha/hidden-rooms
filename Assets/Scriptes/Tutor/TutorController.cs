using Coffee.UIExtensions;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class TutorController : MonoBehaviour
{
    [Inject] private ManagerData _managerData;
    [Inject] private TutorControllerData _tutorData;
    [Inject] private List<IFit> _fitList;
    [Inject] private DiContainer _container;

    [SerializeField] private TextMeshProUGUI _textCharacter;
    [SerializeField] private TextMeshProUGUI _textSimple;

    [SerializeField] private Unmask _unmask;

    [SerializeField] private RectTransform _character;
    [SerializeField] private Button _screenBtn;
    [SerializeField] private RectTransform[] _pillarPoints;

    private Image _imageMask;
    private RectTransform CanvasRect;
    private IFit _targetFit;
    private bool _isNoneRect = false;
    private RectTransform _rectUnmask;
    private Sprite _defaultSprite;

    private void Awake()
    {
        CanvasRect = GetComponent<RectTransform>();
        _imageMask = _unmask.GetComponent<Image>();
        _defaultSprite = _imageMask.sprite;
        _rectUnmask = _unmask.GetComponent<RectTransform>();
    }

    private void Start()
    {
        Invoke("CheckLevel", .01f);
    }

    private void CheckLevel()
    {
        Debug.Log("check");
        if (_managerData.IsTutor)
            gameObject.SetActive(false);
        else
        UpdateTutor();
    }

    private Tutor _currentTutor;


    public void UpdateTutor()
    {
        if(_targetFit != null)
            (_targetFit as FitMask).SpriteInvisible();

        if (_managerData.IsTutor) return;

        if (_currentTutor == null || _currentTutor.QueueTutors.Count <= 0)
        {
            _fitList.Remove(_targetFit);
            if (_targetFit is FitMask && (_targetFit as FitMask).LoopHand)
            {
                Destroy((_targetFit as FitMask).gameObject);
            }
            _currentTutor = _tutorData.GetTutor();
        }
        var currentText = _currentTutor.QueueTutors.Dequeue();

        if (_currentTutor.IsMask || _fitList.Any(f => f.TutorType == _currentTutor.Type && f.IsShowMask))
        {
            CheckBind();

            _targetFit = _fitList
                .Where(f => f.TutorType == _currentTutor.Type)
                .FirstOrDefault();

            if(_currentTutor.Type != TutorType.None && _targetFit != null)
                _targetFit.OnUpdateTutor = UpdateTutor; 
            SetSpriteMask();
            SetFitMask();
        }

        if (currentText.Equals(string.Empty))
        {
            gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(true);

        if (_currentTutor.Type == TutorType.None || !_currentTutor.IsMask)
        {
            _unmask.showUnmaskGraphic = false;
            _screenBtn.interactable = true;
            _unmask.gameObject.SetActive(false);
        }
        else if (_currentTutor.Type == TutorType.Gameplay || _currentTutor.Type == TutorType.Drawer || _currentTutor.Type == TutorType.Zoom)
        {
            _screenBtn.interactable = true;
            _unmask.gameObject.SetActive(false);
            _unmask.showUnmaskGraphic = false;
        }
        else
        if (_currentTutor.Type == TutorType.Gameplay_Level)
        {
            if (_managerData.Level == 2)
            {
                _screenBtn.interactable = true;
                _unmask.gameObject.SetActive(true);
                _unmask.showUnmaskGraphic = true;
            }
        }
        else
        {
            _unmask.showUnmaskGraphic = false;
            _screenBtn.interactable = false;
            _unmask.gameObject.SetActive(true);
        }

        _character.gameObject.SetActive(_currentTutor.IsMaskot);

        if (_currentTutor.IsMaskot)
        {
            _textSimple.gameObject.SetActive(false);
            CharacterMove(_currentTutor.moveCharacter);
            _textCharacter.text = currentText;
        }
        else
        {
            if ((_targetFit as FitMask).LoopHand != null)
            {
                Debug.Log(_targetFit.Name);
                (_targetFit as FitMask).LoopHand.gameObject.SetActive(true);
            }
            else
            {
                _textSimple.gameObject.SetActive(true);
                _textSimple.text = currentText;
            }
        }

        if(_currentTutor.IsEndTutor && _currentTutor.QueueTutors.Count <= 0)
        {
            _managerData.IsTutor = true;
        }
    }

    private void SetFitMask()
    {
        if (_targetFit.Rect != null)
        {
            _unmask.fitTarget = _targetFit.Rect;
            _isNoneRect = false;
        }
        else
        {
            _imageMask.SetNativeSize();
            _rectUnmask.sizeDelta = new Vector2(450, 450);
            _unmask.fitTarget = _rectUnmask;
            _isNoneRect = true;
        }
    }

    private void SetSpriteMask()
    {
        if (_targetFit.Sprite)
            _imageMask.sprite = _targetFit.Sprite;
        else
            _imageMask.sprite = _defaultSprite;
    }

    private void CheckBind()
    {
        if (!_fitList.Any(f => f.TutorType == _currentTutor.Type))
        {
            Debug.Log("Resolve");
            _fitList = _container.ResolveAll<IFit>();
        }
    }

    private void LateUpdate()
    {
        SetPositionUnmask();
    }

    private void SetPositionUnmask()
    {
        if (!_isNoneRect) return;
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(_targetFit.Position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        _rectUnmask.anchoredPosition = WorldObject_ScreenPosition;
    }

    private void CharacterMove(MoveCharacterType characterType)
    {
        switch (characterType)
        {
            case MoveCharacterType.None:

                break;
            case MoveCharacterType.Up:
                _character.DOMoveY(_pillarPoints[0].position.y, .1f).SetEase(Ease.Linear);
                break;
            case MoveCharacterType.Middle:
                _character.DOMoveY(_pillarPoints[1].position.y, .1f).SetEase(Ease.Linear);
                break;
            case MoveCharacterType.Down:
                _character.DOMoveY(_pillarPoints[2].position.y, .1f).SetEase(Ease.Linear);
                break;
        }
    }
}