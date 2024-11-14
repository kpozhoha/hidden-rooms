using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ManagerData : MonoBehaviour, ICoin, ISaveData
{
    public UnityEvent OnUpdateData = new UnityEvent();
    [SerializeField] private List<ItemData> _itemDatas;
    
    private PlayerData _playerData = new();
    private InventoryData _inventoryData = new();
    private RoomData _roomData = new();

    public int Coins => _playerData.Coins;
    public int Level => _playerData.Level;
    public InventoryData InventoryData => _inventoryData;
    /// <summary>
    /// Data items (local)
    /// </summary>
    public List<ItemData> ItemDatas => _itemDatas;

    public RoomData RoomData => _roomData; 

    public bool IsTutor { get { return _playerData.IsTutor; } set { _playerData.IsTutor = value; SaveData(); } }
  

    private void Start()
    {
        LoadData();
    }

    private void OnDisable()
    {
        UtilitData.SaveData(_playerData);
    }

    private void LoadData()
    {
        _playerData = UtilitData.GetData<PlayerData>();
        _inventoryData = UtilitData.GetData<InventoryData>("Inventory");
        _roomData = UtilitData.GetData<RoomData>("RoomData");
    }

    public void SaveData(IAwards awards)
    {
        _playerData.Coins += awards.AmountReward;
        _playerData.Level++;
        UtilitData.SaveData(_playerData);
        OnUpdateData?.Invoke();
    }

    public void SaveData()
    {
        UtilitData.SaveData(_playerData);
        OnUpdateData?.Invoke();
    }

    public void SaveData(int price)
    {
        _playerData.Coins -= price;
        UtilitData.SaveData(_playerData);
        OnUpdateData?.Invoke();
    }

    public void SaveData(Type type)
    {
        var typeName = type.Name;

        switch(typeName)
        {
            case "InventoryData":
                UtilitData.SaveData(_inventoryData, "Inventory");
                break;
            case "RoomData":
                Debug.Log(_roomData.BuildingPlaceDatas.Count);
                UtilitData.SaveData(_roomData, "RoomData");
                break;
        }
    }

    public void UpdateAction(UnityAction OnUpdate)
    {
        OnUpdateData.RemoveListener(OnUpdate);
        OnUpdateData.AddListener(OnUpdate);
    }

    public void RemoveBuilding(string name)
    {
        Debug.Log($"after {_roomData.BuildingPlaceDatas.Count}");

        var build = _roomData.BuildingPlaceDatas.Where(b => b.NameItem == name).FirstOrDefault();
        _roomData.BuildingPlaceDatas.Remove(build);
        Debug.Log($"before {_roomData.BuildingPlaceDatas.Count}");
    }
}
