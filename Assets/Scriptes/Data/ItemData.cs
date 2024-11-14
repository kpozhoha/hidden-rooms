using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class ItemData : ScriptableObject, IBuilding
{
    [SerializeField] private string _nameItem;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _price;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private float _deltaHeight;

    public string NameItem => _nameItem;
    public Sprite Sprite => _sprite;
    public GameObject Prefab => _prefab;
    public int Price => _price;
    public Vector2Int Size => _size;
    public float DeltaHeight => _deltaHeight;

}

public interface IBuilding
{
    string NameItem { get; }
    GameObject Prefab { get; }
    Vector2Int Size { get; }
    float DeltaHeight { get; }
    Sprite Sprite { get; }
}