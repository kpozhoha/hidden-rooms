using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class FitMask : MonoBehaviour, IFit, IPointerDownHandler, IPointerUpHandler
{
    [Inject] private ManagerData _data;

    public UnityAction UpdateTutor;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private TutorType _tutorType;
    [SerializeField] private bool _isStartShow = false;
    [SerializeField] private bool _isEndShow = false;
    [SerializeField] private bool _isSprite = true;

    [SerializeField] private LoopHand _loopHand;
    private bool _start = false;

    public string Name => gameObject.name;

    public bool IsStart => _start;


    public RectTransform Rect
    {
        get
        {
            if (gameObject.TryGetComponent<RectTransform>(out RectTransform rectTransform))
                return rectTransform;
            else
                return null;
        }
    }

    private void OnEnable()
    {
        if (_isStartShow && _data.Level == 2)
        {
            UpdateTutor?.Invoke();
        }
    }

    private void OnDisable()
    {
        if (_isEndShow)
        {
            UpdateTutor?.Invoke();
            Debug.LogWarning($"{gameObject.name} END");
            Destroy(this.gameObject);
        }
        _start = false;
    }

    public void SpriteInvisible()
    {
        if(_sprite != null && !_isSprite)
        {
            if (gameObject.TryGetComponent(out Image image))
                image.color = new Color(0,0,0,0);
        }
    }

    public Sprite Sprite
    {
        get 
        {
            if (_isSprite)
            {
                if (_sprite == null)
                    if (gameObject.TryGetComponent<Image>(out Image image))
                    {
                        _sprite = image.sprite;
                    }
            }
            else
            {
                if (gameObject.TryGetComponent<Image>(out Image image))
                    image.color = Color.white;            
            }
            return _sprite; 
        }
        set
        {
            _sprite = value;
        }
    } 

    public TutorType TutorType { get => _tutorType; set => _tutorType = value; }

    public UnityAction OnUpdateTutor { set
        {

            UpdateTutor = value;
            _start = true;
            if (gameObject.TryGetComponent<Button>(out var button))
            {
                button.onClick.AddListener(UpdateTutor);
            }
        }
    }

    public Vector3 Position => gameObject.transform.position;

    public bool IsShowMask { get => _isStartShow; set => _isStartShow = value; }

    public bool IsEndShow { get => _isEndShow; set => _isEndShow = value; }
    public LoopHand LoopHand => _loopHand; 

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " puf");
        if (gameObject.TryGetComponent<Button>(out var button))
        {
            button.onClick.AddListener(() => UpdateTutor?.Invoke());
            //button.onClick.Invoke();
            return;
        }
        UpdateTutor?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateTutor = null;
        _start= false;
        //Destroy(this);
    }

}

public interface IFit
{
    UnityAction OnUpdateTutor { set; }
    bool IsShowMask { get; }
    string Name { get; }
    RectTransform Rect { get; }
    Sprite Sprite { get; }
    TutorType TutorType { get; }
    Vector3 Position { get; }
}

public enum MoveCharacterType
{
    None,
    Up,
    Middle,
    Down
}

public enum TutorType
{
    None,
    Play,
    Room,
    Room_Inventory,
    Room_Inventory_Item,
    Room_Shop,
    Room_Shop_Item,
    Gameplay,
    Gameplay_Item,
    Gameplay_Level,
    Gameplay_Level_End,
    Go_Menu,
    Back,
    Drawer,
    Zoom
}