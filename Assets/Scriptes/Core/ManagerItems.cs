using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ManagerItems : MonoBehaviour, IGetRay
{
    [SerializeField] private ItemPool itemPool;

    private IListView _listView;
    private Ray _targetRay;
    private Vector3 _startPoint = Vector3.zero;

    private void Awake()
    {
        _startPoint = Camera.main.transform.position;
    }

    public void Construct(LevelData levelData, IListView listView, UnityAction OnCheckLevel)
    {
        itemPool = new(levelData.ItemDatas.Select(d => d.NameItem).ToList(), OnCheckLevel);
        _listView = listView;
    }

    public void GetRay(Vector3 positionTouch)
    {
        Debug.Log("Ray");
        _targetRay = Camera.main.ScreenPointToRay(positionTouch);

        Physics.Raycast(_targetRay, out RaycastHit hit, 100, 1 << 6);

        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<IItem>(out var item))
        {
            if (_listView.UpdateView(item.Name))
            {
                itemPool.OnCheckItem(item, _listView.DeleteView);                     // предаем через интерфейм только тот функционал, который будем использовать
            }
            else
                item.OnWrong();
            Debug.Log(itemPool.Count);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_targetRay);
    }
#endif
}
