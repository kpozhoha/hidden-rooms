using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class RotatingRoom : MonoBehaviour, IRotatingRoom
{
    [SerializeField] private ParticleSystem _particleSystem;
    private Tween _rotateTween;
    private Quaternion _startRotate;

    private void Start()
    {
        _startRotate = transform.rotation;
    }

    public void OnStartRotate(UnityAction OnInit)
    {
        _rotateTween = transform
            .DOShakeRotation(2f, Vector3.up * 90, 10, 90, true, ShakeRandomnessMode.Harmonic)
            .OnKill(() => OnInit?.Invoke());
    }

    public void OnRotation(Transform target, UnityAction OnEndLevel)
    {
        _rotateTween = transform
            .DORotate(Vector3.up * 90, .1f, RotateMode.LocalAxisAdd)
            .SetDelay(1)
            .SetEase(Ease.Linear)
            .SetLoops(5)
            .OnKill(() =>
            {
                transform.rotation = _startRotate;
                transform
                    .DOShakeScale(.5f, 1, 9)
                    .SetEase(Ease.OutCirc)
                    .OnKill(() =>
                    {
                        _particleSystem.Play();
                        target
                            .DOMoveY(100, 2)
                            .SetEase(Ease.Linear)
                            .OnKill(() =>
                            {
                                OnEndLevel?.Invoke();
                            });
                    });

            });

    }
}