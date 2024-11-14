using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;

public class LevelManager : MonoBehaviour
{
    public enum TypeLevel
    {
        Icon, 
        Name
    }

    public UnityAction OnStartDrawer;

    [Inject] private DiContainer _container;

    [Header("Managers")]
    [SerializeField] private ManagerItems _managerItems;
    [SerializeField] private RotatingRoom _rotatingRoom;
    [SerializeField] private ItemsListView _itemsListView;

    [SerializeField] private LevelPanel _levelPanel;

    [Header("List All Levels")]
    public List<LevelData> LevelData;

    private InputsPlayer.Drag.InputPlayer _inputPlayerDrag;

    private LevelData _currentLevel;
    private ISaveData _saveData;

    private int _currentIndex = 0;
    private bool _isEnd = false;

    private Camera _camera;

    [Inject]
    private void Construct(ManagerData managerData)
    {
        _saveData = managerData;
        _currentIndex = (managerData.Level - 1) % LevelData.Count;
    }

    private void Awake()
    {
        CreateLevel();
        _levelPanel.NextLevel = NextLevel;
        _camera = Camera.main;
        _inputPlayerDrag = new(_managerItems, _camera, _currentLevel.transform, 10);
    }

    private void Update()
    {
        if(!_isEnd)
        _inputPlayerDrag.OnInput();
    }

    // создать объект уровня
    // передать его данные в менеджер предметов и в отображение
    public void CreateLevel()
    {
        _currentLevel = _container.InstantiatePrefabForComponent<LevelData>(LevelData[_currentIndex], _rotatingRoom.transform);

        //_itemsListView.CreateListView(_currentLevel);
        //_managerItems.Construct(_currentLevel, _itemsListView, OnCheckLevel);

        if (_inputPlayerDrag != null)
            _inputPlayerDrag.Target = _currentLevel.transform;
        _isEnd = false;

    }

    public void OnCheckLevel()
    {
        int index = _currentIndex % LevelData.Count;
        
        _rotatingRoom.OnRotation(_currentLevel.transform, () =>
        {
            if (index <= 2)
                GlobalEvents.UpdateTutor?.Invoke();
            _levelPanel.ShowCanvas(LevelData[index]);
            _itemsListView.gameObject.SetActive(false);
            _saveData.SaveData(LevelData[index]);
            _camera.fieldOfView = 60;
        });
        _isEnd = true;
    }

    public void OnCheckLevel(int amountQueue)
    {
        if (amountQueue <= 0)
        {
            _levelPanel.ShowCanvas(LevelData[0]);
        }
    }

    public void OnStartLevel(UnityAction UpdateTutor)
    {
        _rotatingRoom.OnStartRotate(() => { 
            InitPanelItems();
            if(_currentIndex + 1 < 2)
            UpdateTutor?.Invoke(); });
    }

    private void InitPanelItems()
    {
        this.enabled= true;
        _itemsListView.CreateListView(_currentLevel);
        _managerItems.Construct(_currentLevel, _itemsListView, OnCheckLevel);
    }

    private void NextLevel()
    {
        Destroy(_currentLevel.gameObject);
        _itemsListView.DelletAllView();
        //_itemsListView.gameObject.SetActive(true);

        _currentLevel = null;
        _currentIndex = (++_currentIndex) % LevelData.Count;

        if (_currentLevel == null)
        {
            CreateLevel();
        }

        OnStartDrawer?.Invoke();
    }

}