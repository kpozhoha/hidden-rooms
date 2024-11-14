using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemsListView : MonoBehaviour, IListView
{
    public Action<int> OnCheckLevel;

    [SerializeField] private ItemView _prefabItemView;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem _StartParticleSystem;

    [SerializeField] private Transform _container;                      // объект, в котором будем создавать отображение искомых объектов

    private Queue<ItemView> _items = new Queue<ItemView>();             // а вот другой вопрос с теми, что ждут очереди

    private List<ItemView> _itemsView = new List<ItemView>();           // так как нужно с рандомной позиции изъять (я лох) только для видимых
    
    private List<ItemView> _itemsDel = new List<ItemView>();           // так как нужно с рандомной позиции изъять (я лох) только для видимых


    private string _currentNameView = "";
    [SerializeField] private GridLayoutGroup _layoutGroup;


    // реализовать метод, который будет принимать список данных о предметах из LevelManager
    public void CreateListView(LevelData data) 
    {
        gameObject.SetActive(true);
                                                                        // лучше всего создать сразу все, но сделать видимыми только нужное количество. О-оптимизация
        data.ItemDatas.ForEach(dataItem =>
        {
            var item = Instantiate(_prefabItemView, _container).GetComponent<ItemView>();
            item.Initialize(dataItem, data.TypeLevel);
            _items.Enqueue(item);
        });

        _layoutGroup.constraintCount = data.ItemDatas.Count % 2 == 0 ? 4 : 3;

        OnVisibleView(data.AmountItemsView);
    }

    // будем принимать только количество необходимое для отображения
    private void OnVisibleView(int countVisible)
    {
        for (int i = 0; i < countVisible; i++)
        {
            UpdateView();
        }
    }

    // обновление листа отображения предметов
    public bool UpdateView(string name)
    {
        if (!_itemsView.Any(item => item.Name == name))
            return false;
        _currentNameView = name;

        return true;
    }

    // обновление очереди предметов
    public void UpdateView(float delay = 0)
    {
        if (_items.Count > 0)
        {
            var item = _items.Dequeue(); // извлекаем из очереди
            item.OnVisible(delay, _StartParticleSystem);
            _itemsView.Add(item);
            _currentNameView = "";
        }
    }

    public void DeleteView()
    {
        var delItem = _itemsView
            .Where(i => i.Name == _currentNameView)
            .First()
            .Remove();
        _itemsView.Remove(delItem);
        _itemsDel.Add(delItem);

        var particle = Instantiate(_particleSystem, delItem.transform, false);
        particle.transform.localScale = Vector3.one * 150;

        UpdateView(1);
    }

    public void DelletAllView()
    {
        _itemsDel.ForEach(i => Destroy(i.gameObject));
        _itemsDel.Clear();
    }

    public void DeleteView(string nameItem)
    {
        var delItem = _itemsView
            .Where(i => i.Name == nameItem)
            .First()
            .Remove();
        _itemsView.Remove(delItem);
        _itemsDel.Add(delItem);

        var particle = Instantiate(_particleSystem, delItem.transform, false);
        particle.transform.localScale = Vector3.one * 150;

        UpdateView(1);
    }
}
