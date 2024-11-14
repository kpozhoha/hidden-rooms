using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public class ItemPool
{
    [SerializeField] private List<string> _itemsName = new();
    private UnityAction OnCheckLevel;

    public ItemPool(List<string> itemsName, UnityAction OnCheck)
    {
        _itemsName = itemsName;
        OnCheckLevel = OnCheck;
    }

    public int Count => _itemsName.Count;

    // метод который проверяет есть выбранный предмет в списке искомых
    public void OnCheckItem(IItem item)
    {
        if (_itemsName.Any(i => i.Equals(item.Name)))
        {
            FindItem(item.Name);                        // удаляем из списка
            if (_itemsName.Count > 0)
                item.OnFind(null);
            else
                item.OnFind(null, OnCheckLevel);
        }
    }

    public void OnCheckItem(IItem item, UnityAction<string> OnRemoveView)
    {
        if (_itemsName.Any(i => i.Equals(item.Name)))
        {
            FindItem(item.Name);                        // удаляем из списка
            if (_itemsName.Count > 0)
                item.OnFind(OnRemoveView);
            else
                item.OnFind(OnRemoveView, OnCheckLevel);
        }
    }

    // нужен метод, который будет или помечать, что предмет найден, или удалять его из списка
    public void FindItem(string name)
    {
        _itemsName.Remove(name);
    }
}
