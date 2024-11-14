using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ShopController : MonoBehaviour
{
    [SerializeField] private ShopItemView _prefab;
    [SerializeField] private Transform _container;
    [SerializeField] private List<Sprite> _bgImages;

    [SerializeField] private List<ItemData> _itemsData;
    [SerializeField] private int _countVisibleItems = 4;

    [Inject] private ManagerData _managerData;
    [Inject] private DiContainer _diContainer;

    private List<ShopItemView> _currentItems = new();

    private void OnEnable()
    {
        OnVisibleItems();
    }

    private void OnVisibleItems()
    {
        for (int i = 0; i < _countVisibleItems; i++)
        {
            var item = _diContainer.InstantiatePrefabForComponent<ShopItemView>(_prefab, _container);
            _currentItems.Add(item);
            var data = _itemsData[i % _itemsData.Count];
            item.Init(data, BuyItem);
        }

        if (!_managerData.IsTutor)
        {
            var tutorItem = _currentItems.FirstOrDefault();
            var fit = _diContainer.InstantiateComponent<FitMask>(tutorItem.BuyButton.gameObject);
            fit.Sprite = tutorItem.Sprite;
            fit.TutorType = TutorType.Room_Shop_Item;
            _diContainer.Bind<IFit>().FromInstance(fit).AsCached().NonLazy();
        }
    }

    private void BuyItem(ItemData itemData, GameObject obj)
    {
        if (itemData.Price > _managerData.Coins)
            return;
        _managerData.SaveData(itemData.Price);
        _managerData.InventoryData.AddItem(itemData.NameItem);
        _managerData.SaveData(typeof(InventoryData));
        Destroy(obj);
    }
}
