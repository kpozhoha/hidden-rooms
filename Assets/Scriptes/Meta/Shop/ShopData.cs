using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : MonoBehaviour
{
    [SerializeField] private List<string> _items;

    public List<string> Items => _items; 

    public void AddNewItem(string nameItem)
    {
        _items.Add(nameItem);
    }
}
