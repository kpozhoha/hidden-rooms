using UnityEngine;
using UnityEngine.Events;

public interface IRotatingRoom
{
    void OnRotation(Transform target, UnityAction OnEndLevel);
}