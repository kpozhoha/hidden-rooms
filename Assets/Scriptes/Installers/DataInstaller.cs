using UnityEngine;
using Zenject;

public class DataInstaller : MonoInstaller
{
    [SerializeField] private ManagerData _managerData;
    [SerializeField] private TutorControllerData _tutorControllerData;

    public override void InstallBindings()
    {
        BindPlayerData();

        BindTutorData();
    }

    private void BindTutorData()
    {
        Container
            .Bind<TutorControllerData>()
            .FromInstance(_tutorControllerData)
            .AsSingle()
            .NonLazy();
    }

    private void BindPlayerData()
    {
        var data = Container.InstantiatePrefabForComponent<ManagerData>(_managerData);

        Container
            .Bind<ManagerData>()
            .FromInstance(data)
            .AsSingle()
            .NonLazy();
    }
}