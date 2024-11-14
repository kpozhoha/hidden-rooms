using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class CoinsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private ICoin _coins;

    private void OnEnable()
    {
        UpdateData();
    }

    [Inject]
    private void Construct(ManagerData managerData)
    {
        _coins = managerData;
        //UpdateData();
        _coins.UpdateAction(UpdateData);
    }

    public void UpdateData()
    {
        _coinsText.text = _coins.Coins.ToString();
    }
}
