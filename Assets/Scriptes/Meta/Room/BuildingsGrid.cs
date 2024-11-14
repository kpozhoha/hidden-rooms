using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;
using Color = UnityEngine.Color;

public class BuildingsGrid : MonoBehaviour, IGetRay
{
    public UnityAction<Building> OnBuildes;
    public UnityAction<Building> OnRemoveBuilding;

    public Vector2Int GridSize = new Vector2Int(10, 10);
    [SerializeField] private float _time = 0.5f;

    private Building[,] grid;
    private Building flyingBuilding;
    private BuildingView _buildingView;
    private Camera mainCamera;

    private Ray _targetRay;
    private float _startTime;

    private float _height = 0;

    public Building[,] Grid => grid; 

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];

        mainCamera = Camera.main;
    }

    private void Update()
    {
        FlyingBuildItemMobile();

        if (flyingBuilding == null)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                switch(touch.phase)
                {
                    case TouchPhase.Began:
                        _startTime = Time.time;
                        break;
                    case TouchPhase.Stationary:
                        if (Time.time - _startTime > _time)
                        {
                            GetRay(touch.position);
                        }
                        break;
                }
            }
        }
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab, transform, false);
        flyingBuilding.transform.position = new Vector3(3,0,3);

    }

    public void StartPlacingBuilding(Building buildingPrefab, BuildingView buildView)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab, transform, false);
        flyingBuilding.OnRevertView.AddListener(buildView.RevertView);
    }

    private bool IsExistsOnGrid(IBuilding building)
    {
        bool isExists = false;
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                if (grid[x,y] != null && grid[x, y].name == building.NameItem)
                    isExists = true;
                    break;
            }
        }
        return isExists;
    }

    public void StartPlacingBuilding(IBuilding building, UnityAction DropItem)
    {
        if (IsExistsOnGrid(building))
        {
            Debug.Log("exists");
            DropItem?.Invoke();
            return;
        }
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        var build = Instantiate(building.Prefab, transform, false).AddComponent<Building>();
        build.Size = building.Size;
        build.name = building.NameItem;
        _height = building.DeltaHeight;
        flyingBuilding = build;

        flyingBuilding.OnRevertView.AddListener(DropItem);

    }

    public void StartPlacingBuilding(IBuilding building, BuildingPlaceData placeData, UnityAction DropItem)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        var build = Instantiate(building.Prefab, transform, false).AddComponent<Building>();
        build.Size = building.Size;
        build.name = building.NameItem;

        build.transform.position = placeData.Position;
        _height = building.DeltaHeight;
        flyingBuilding = build;
        flyingBuilding.OnRevertView.AddListener(DropItem);
        LoadPlaceFlyingBuilding((int)placeData.Position.x, (int)placeData.Position.z);
    }

    public void GetRay(Vector3 positionTouch)
    {
        Debug.Log("Ray");
        _targetRay = Camera.main.ScreenPointToRay(positionTouch);

        Physics.Raycast(_targetRay, out RaycastHit hit, 100, 1 << 6);

        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<Building>(out var building))
        {
            flyingBuilding = building;
            RemovePlaceFlyingBuilding(building);
        }
    }

    private void FlyingBuildItem()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;

                if (available && IsPlaceTaken(x, y)) available = false;

                flyingBuilding.transform.position = new Vector3(x, 0, y);
                flyingBuilding.SetTransparent(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }
    }

    private void FlyingBuildItemMobile()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero); // создание плоскости 

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);


                Ray ray = mainCamera.ScreenPointToRay(touch.position);

                if (groundPlane.Raycast(ray, out float position))
                {
                    Vector3 worldPosition = ray.GetPoint(position);

                    int x = Mathf.RoundToInt(worldPosition.x);
                    int y = Mathf.RoundToInt(worldPosition.z);

                    bool available = true;

                    if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                    if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;

                    if (available && IsPlaceTaken(x, y)) available = false;
                    flyingBuilding.transform.position = new Vector3(x, 0, y);

                    var localPos = flyingBuilding.transform.localPosition;
                    localPos.y = _height;
                    flyingBuilding.transform.localPosition = localPos;

                    flyingBuilding.SetTransparent(available);

                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (available)
                            PlaceFlyingBuilding(x, y);
                        else
                        {
                            flyingBuilding.DropBuilding();
                        }                 
                    }
                }
            }
        }
    }

    // проверка пусто ли место
    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null)
                {
                    Debug.Log(grid[placeX + x, placeY + y].name);
                    return true;
                }
            }
        }

        return false;
    }

    // установка на пустое место
    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }

        flyingBuilding.SetNormal(new Vector2Int(placeX, placeY));
        OnBuildes(flyingBuilding);
        flyingBuilding = null;
        GlobalEvents.UpdateTutor?.Invoke();
    }

    private void LoadPlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }

        flyingBuilding.SetNormal(new Vector2Int(placeX, placeY));
        flyingBuilding = null;
    }

    public void RemovePlaceFlyingBuilding(Building building)
    {
        OnRemoveBuilding(building);
        Debug.Log(building.name);
                Debug.Log($"x {grid.GetLength(0)} y {grid.GetLength(1)}");
        //for (int x = 0; x < building.Size.x; x++)
        //{
        //    for (int y = 0; y < building.Size.y; y++)
        //    {
        //        Debug.Log($"x {building.StartPlace.x + x} y {building.StartPlace.y + y}");
        //        //grid[building.StartPlace.x + x, building.StartPlace.y + y] = null;
        //    }
        //}

        for(int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                if (grid[x, y] && grid[x, y].name == flyingBuilding.name)
                {
                    grid[x, y] = null;
                }
                //if (grid[x, y])
                //    Debug.Log($"{grid[x, y].name} {x} : {y}");
            }
        }

    }

    public void RemovePlaceFlyingBuilding()
    {
        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                if (grid[i, j] == flyingBuilding)
                {
                    Debug.Log(grid[i, j].name);
                    grid[i, j] = null;
                }
            }
        }
        OnRemoveBuilding(flyingBuilding);
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                if ((x + y) % 2 == 0) Gizmos.color = new Color(0.98f, 0f, 1f, 0.3f);
                else Gizmos.color = new Color(1f, 0.78f, 0f, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
}