using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    [SerializeField] private Image _iconItem;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;

    public Sprite Sprite { get {
            _buyButton.TryGetComponent<Image>(out var image);
            return image.sprite;
        }  
    }

    public Button BuyButton => _buyButton; 

    private void OnDisable()
    {
        _buyButton.onClick.RemoveAllListeners();
        Destroy(gameObject);
    }

    public void Init(int price, Sprite icon, UnityAction OnBuy)
    {
        _priceText.text = price.ToString();
        _iconItem.sprite = icon;
        _buyButton.onClick.AddListener(OnBuy);
    }

    public void Init(ItemData item, UnityAction<ItemData, GameObject> OnBuy)
    {
        _priceText.text = item.Price.ToString();
        _iconItem.sprite = item.Sprite;
        _buyButton.onClick.AddListener(() => {
            OnBuy(item, this.gameObject);
            GlobalEvents.UpdateTutor?.Invoke();
        });
    }


}
