using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public UnityAction<IBuilding, UnityAction> OnBuild;
    public UnityAction<ItemData> OnRevert;


    [SerializeField] private Image _background;
    [SerializeField] private Image _iconItem;

    private Building _building;
    private ItemData _itemData;
    private Transform _container;

    private void Start()
    {
        _container = gameObject.GetComponentInParent<Transform>();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public ItemData ItemData { get => _itemData; 
        set
        {
            _itemData = value;
            SetViewData();
        } 
    }

    public Sprite Background => _background.sprite;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        OnBuild(_itemData, RevertView);
        GlobalEvents.UpdateTutor?.Invoke();
        gameObject.SetActive(false);
        Debug.Log("Drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end Drag");
    }

    public void RevertView()
    {
        gameObject.SetActive(true);
    }

    private void SetViewData()
    {
        _iconItem.sprite = _itemData.Sprite;
    }
}
