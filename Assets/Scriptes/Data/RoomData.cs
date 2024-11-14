using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    //save items who buys
    [SerializeField] private Building[,] _grid;
    [SerializeField] private List<BuildingPlaceData> buildingPlaceDatas = new();

    public RoomData()
    {
    }

    public Building[,] Grid => _grid; 
    public List<BuildingPlaceData> BuildingPlaceDatas => buildingPlaceDatas;

    public List<ItemData> GetItems(List<ItemData> datas)
    {
        List<ItemData> itemDatas = new List<ItemData>();
        List<string> names = buildingPlaceDatas.Select(place => place.NameItem).ToList();

        names.ForEach(n =>
        {
            var item = datas.Where(d => d.NameItem == n).First();

            itemDatas.Add(item);
        });

        return itemDatas;
    }

    public void AddBuilding(Building building, Building[,] grid)
    {
        buildingPlaceDatas.Add(new BuildingPlaceData(building.transform.position, building.name));
        _grid = grid;
    }

    public void RemoveBuilding(string name, Building[,] grid)
    {
        Debug.Log($"after {buildingPlaceDatas.Count}");

        var build = buildingPlaceDatas.Where(b => b.NameItem == name).FirstOrDefault();
        buildingPlaceDatas.Remove(build);
        Debug.Log($"before {buildingPlaceDatas.Count}");
        _grid = grid;
    }
}

[System.Serializable]
public struct BuildingPlaceData
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private string _nameItem;

    public BuildingPlaceData(Vector3 position, string nameItem)
    {
        _position = position;
        _nameItem = nameItem;
    }

    public Vector3 Position => _position;
    public string NameItem => _nameItem; 
}