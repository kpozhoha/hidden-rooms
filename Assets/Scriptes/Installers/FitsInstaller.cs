using UnityEngine;
using Zenject;

public class FitsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(IFit), typeof(FitMask)).FromComponentsInHierarchy().AsCached();
    }
}