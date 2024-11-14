using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShopItem : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Building _prefab;
    [SerializeField] private Sprite _spriteItem;
    [SerializeField] private int _price;

    public string Name => _name; 
    public Building Prefab => _prefab; 
    public Sprite SpriteItem => _spriteItem; 
    public int Price => _price; 
}
