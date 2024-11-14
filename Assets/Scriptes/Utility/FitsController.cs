using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class FitsController : MonoBehaviour
{
    [SerializeField] private List<IFit> _fits;

    [Inject] 
    private void Construct(List<IFit> fit)
    {
        _fits = fit;
    }
}
