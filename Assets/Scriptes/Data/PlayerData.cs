using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    [SerializeField] private int _level;
    [SerializeField] private int _coins;
    [SerializeField] private bool _isTutor;

    public PlayerData()
    {
        _isTutor = false;
        _level = 1;
        _coins = 650;
    }

    public int Coins { get => _coins; set => _coins = value; }
    public int Level { get => _level; set => _level = value; }
    public bool IsTutor { get => _isTutor; set => _isTutor = value; }
}
