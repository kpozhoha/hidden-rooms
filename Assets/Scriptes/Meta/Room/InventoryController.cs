using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class InventoryController : MonoBehaviour
{
    [Inject] private ManagerData _managerData;
    [Inject] private DiContainer _diContainer;

    public static UnityEvent<IBuilding> OnRevertView = new UnityEvent<IBuilding>();

    [SerializeField] private BuildingView _prefab;
    [SerializeField] private Transform _container;
    [SerializeField] private BuildingsGrid _grid;

    [SerializeField] private List<BuildingView> _buildingViews = new();

    private void OnEnable()
    {
        _grid.OnBuildes = AddToRoomData;
        _grid.OnRemoveBuilding = AddToInventoryData;

        OnRevertView.AddListener(CreateView);
        Initialize();
    }

    private void OnDisable()
    {
        OnRevertView.RemoveListener(CreateView);

        _grid.OnBuildes = null;
        _grid.OnRemoveBuilding = null;

        if(_buildingViews.Any(v => v != null))
        _buildingViews.ForEach(v =>
        {
            Destroy(v.gameObject);
        });
        _buildingViews.Clear();

    }

    private void Start()
    {
        LoadRoomData();
    }

    private void Initialize()
    {
        var datas = _managerData.InventoryData.GetItems(_managerData.ItemDatas);

        datas.ForEach(d =>
        {
            var view = Instantiate(_prefab, _container, false);
            view.ItemData = d;
            view.OnBuild = _grid.StartPlacingBuilding;

            _buildingViews.Add(view);
        });

        if (!_managerData.IsTutor)
        {
            var view = _buildingViews.FirstOrDefault();
            Debug.Log(view.gameObject.name);
            var fit = _diContainer.InstantiateComponent<FitMask>(view.gameObject);
            fit.Sprite = view.Background;
            fit.TutorType = TutorType.Room_Inventory_Item;
            _diContainer.Bind<IFit>().FromInstance(fit).AsCached().NonLazy();
        }
    }

    private void LoadRoomData()
    {
        var positions = _managerData.RoomData.BuildingPlaceDatas;
        var datas = _managerData.RoomData.GetItems(_managerData.ItemDatas);

        if (datas.Count < 0) return;

        datas.ForEach(d =>
        {
            var view = Instantiate(_prefab, _container, false);
            view.ItemData = d;
            view.OnBuild = _grid.StartPlacingBuilding;

            var position = positions.Where(p => p.NameItem == d.NameItem).FirstOrDefault();
            _grid.StartPlacingBuilding(view.ItemData, position, view.RevertView);
            view.gameObject.SetActive(false);
        });
    }

    private void CreateView(IBuilding data)
    {
        Debug.Log("Create");
        var view = Instantiate(_prefab, _container, false);
        view.ItemData = data as ItemData;
        view.OnBuild = _grid.StartPlacingBuilding;

        _buildingViews.Add(view);
    }

    private void AddToRoomData(Building building)
    {
        _managerData.RoomData.AddBuilding(building, _grid.Grid);
        _managerData.InventoryData.RemoveItem(building.name);
        SaveData();

    }

    private void AddToInventoryData(Building building)
    {
        //_grid.RemovePlaceFlyingBuilding(building);
        
        _managerData.RemoveBuilding(building.name);
        _managerData.InventoryData.AddItem(building.name);
        SaveData();
    }

    private void SaveData()
    {
        _managerData.SaveData(typeof(RoomData));
        _managerData.SaveData(typeof(InventoryData));
    }
}
