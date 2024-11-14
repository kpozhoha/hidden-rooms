using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.ParticleSystem;

public class ItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameItem;
    [SerializeField] private Image _iconItem;
    [SerializeField] private Transform _iconBG;

    public string Name => _nameItem.text;

    // ћетод который будет инициализировать параметры нашего предмета и задавать параметры отображени€. ѕримитивна€ абстрактна€ фабрика типа
    public void Initialize(ItemData data, LevelManager.TypeLevel typeLevel)
    {
        switch (typeLevel)
        {
            case LevelManager.TypeLevel.Icon:
                //OnScaleIcon(data.GetPreviewSprite());
                _iconItem.sprite = data.Sprite;

                _iconBG.gameObject.SetActive(true);
                _iconItem.gameObject.SetActive(true);
                break;
            case LevelManager.TypeLevel.Name:
                //_nameItem.text = data.ItemName;
                _nameItem.gameObject.SetActive(true);
                break;
        }
        _nameItem.text = data.NameItem;
    }

    public void OnVisible(float delay)
    {
        transform.localScale = Vector3.zero;

        transform
            .DOScale(1, .5f)
            .SetDelay(delay)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                gameObject.SetActive(true);
            })
            .OnKill(() => gameObject.SetActive(true));

        //gameObject.SetActive(true);
    }

    public void OnVisible(float delay, ParticleSystem particleSystem)
    {
        transform.localScale = Vector3.zero;
        _iconItem.gameObject.SetActive(false);

        transform
            .DOScale(1, .5f)
            .SetDelay(delay)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                gameObject.SetActive(true);
            })
            .OnKill(() =>
            {
                _iconItem.color = new Color(0, 0, 0, 0);
                _iconItem.gameObject.SetActive(true);
                var particle = Instantiate(particleSystem, _iconItem.transform, false);
                particle.transform.localScale = Vector3.one * 100;
                _iconItem
                .DOColor(new Color(0, 0, 0, 1), .5f)
                .OnKill(() => Destroy(particle.gameObject)); 
            });

        //gameObject.SetActive(true);
    }

    // позже изменить
    public ItemView Remove()
    {
        _iconItem
            .DOColor(new Color(1, 1, 1, 1), .5f);
        
        return this;
    }

    private void OnScaleIcon(Sprite sprite)
    {
        var height = _iconItem.gameObject.GetComponent<RectTransform>().rect.height;
        _iconItem.sprite = sprite;
        _iconItem.SetNativeSize();


        float natHeight = sprite.rect.height;
        float deltaScale =  height / natHeight;
        float width = sprite.rect.width * deltaScale;

        Vector2 size = new Vector2(width, height);

        _iconItem.rectTransform.sizeDelta = size;

    }
}
