using UnityEngine;
using DG.Tweening;

public class CursorMove : MonoBehaviour
{
    [SerializeField] private float _targetScale;
    [SerializeField] private RectTransform _handRect;

    private RectTransform _rect;
    private Tween _tween;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Follow();
        if(Input.GetMouseButtonDown(0))
        {
            TapHandDown();
        }
        else 
            if(Input.GetMouseButtonUp(0))
        {
            TapHandUp();
        }
    }

    private void Follow()
    {
        _rect.position = Vector3.MoveTowards(_rect.position, Input.mousePosition, 500*Time.deltaTime);
    }

    private void TapHandDown()
    {
        if(_tween == null)
        _tween = _handRect
            .DOScale(_targetScale, .25f)
            .SetEase(Ease.Linear)
            //.SetLoops(2, LoopType.Yoyo)
            .OnKill(() => {
                _tween = null;
                Debug.Log("end"); });
           
    }

    private void TapHandUp()
    {
            _tween = _handRect
                .DOScale(1, .25f)
                .SetEase(Ease.Linear)
                //.SetLoops(2, LoopType.Yoyo)
                .OnKill(() => {
                    _tween = null;
                    Debug.Log("end");
                });

    }
}
