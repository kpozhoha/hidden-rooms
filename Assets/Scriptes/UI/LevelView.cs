using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    private ManagerData _managerData;

    private void OnEnable()
    {
        UpdateData();
    }

    [Inject]
    private void Costruct(ManagerData managerData)
    {
        _managerData = managerData;
        _managerData.UpdateAction(UpdateData);
    }

    public void UpdateData()
    {
        _levelText.text = _managerData.Level.ToString();
    }
}
