using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    [SerializeField] private List<string> _itemsName = new List<string>();

    public InventoryData()
    {
        _itemsName = new List<string>();
    }

    public List<string> ItemsName => _itemsName; 

    public List<ItemData> GetItems(List<ItemData> datas)
    {
        Debug.Log($"names {_itemsName.Count}");
        List<ItemData> itemDatas = new List<ItemData>();
        _itemsName.ForEach(n =>
        {
            var item = datas.Where(d => d.NameItem == n).First();

            itemDatas.Add(item);
        });

        return itemDatas;
    }

    public void AddItem(string nameItem)
    {
        _itemsName.Add(nameItem);
    }

    public void RemoveItem(string nameItem)
    {
        _itemsName.Remove(nameItem);
    }
}
