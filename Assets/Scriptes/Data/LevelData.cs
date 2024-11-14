using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelData : MonoBehaviour, IAwards
{
    [Inject] private DiContainer _container;

    [SerializeField] private List<Item> items;
    [SerializeField] private int _amountItemsView;
    [SerializeField] private LevelManager.TypeLevel _typeLevel;
    [SerializeField] private int _amountReward;
    [SerializeField] private string _nameLevel;

    public List<ItemData> ItemDatas => items.Select(item => (item as IItem).Data).ToList(); // получение списка информации о предметах

    public LevelManager.TypeLevel TypeLevel => _typeLevel;

    public int AmountItemsView => _amountItemsView;

    public int AmountReward => _amountReward;

    public string NameLevel => _nameLevel;

    private void Awake()
    {
        CreateTutor();
    }

    private void CreateTutor()
    {
        if (items.Any(i => i.gameObject.name == "Laptop"))
        {
            var item = items.Where(i => i.gameObject.name == "Laptop").FirstOrDefault();
            var mask = _container.InstantiateComponent<FitMask>(item.gameObject);
            mask.TutorType = TutorType.Gameplay_Item;
            _container.Bind<IFit>().FromInstance(mask).AsCached().NonLazy();
        }
    }
}