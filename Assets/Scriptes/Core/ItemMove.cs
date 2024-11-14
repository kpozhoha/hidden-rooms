using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ItemMove : MonoBehaviour, IItem
{
    [SerializeField] private TypeMove _typeMove;
    [SerializeField] private float _changeValue;
    [SerializeField] private Vector3 _lookAt;

    [SerializeField] private Vector3[] _wayPoints;

    public ItemData Data => null;
    public string Name => name;

    public enum TypeMove
    {
        Scale,
        Move,
        MoveY,
        MoveZ,
        Path,
        RotateX,
        RotateY,
        RotateZ

    }

    public void OnFind(UnityAction<string> OnRemoveView = null, UnityAction OnCheckLevel = null)
    {
    }

    public void OnWrong()
    {
        switch (_typeMove)
        {
            case TypeMove.Scale:
                transform
                    .DOScaleX(_changeValue, 1f)
                    .SetEase(Ease.Linear);
                transform
                    .DOLocalMoveX(106, 1f)
                    .OnKill(() => Destroy(this));
                break;
            case TypeMove.Move:
                transform
                    .DOLocalMove(_lookAt, 1f)
                    .SetEase(Ease.Linear)
                    .OnKill(() => Destroy(this));
                break;
            case TypeMove.MoveY:
                transform
                    .DOMoveY(_changeValue, 1f)
                    .SetEase(Ease.Linear)
                    .OnKill(() => Destroy(this));
                break;
            case TypeMove.MoveZ:
                transform
                    .DOMoveZ(_changeValue, 1f)
                    .SetEase(Ease.Linear)
                    .OnKill(() => Destroy(this));
                break;
            case TypeMove.Path:
                transform
                    .DOLocalPath(_wayPoints, 1)
                    //.SetOptions(true, AxisConstraint.None, AxisConstraint.Y | AxisConstraint.X)
                    .SetLookAt(.1f)
                    .OnKill(() => Destroy(this));
                break;
            case TypeMove.RotateX:
                transform
                    .DORotate(Vector3.right * _changeValue, 1f, RotateMode.LocalAxisAdd)
                    .OnKill(() => Destroy(this));
                break;
            case TypeMove.RotateY:
                transform
                    .DORotate(Vector3.up * _changeValue, 1f, RotateMode.LocalAxisAdd)
                    .OnKill(() => Destroy(this));
                break;
            case TypeMove.RotateZ:
                transform
                    .DORotate(Vector3.forward * _changeValue, 1f, RotateMode.LocalAxisAdd)
                    .OnKill(() => Destroy(this));
                break;
        }
    }
}
