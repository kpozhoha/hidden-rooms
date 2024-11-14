using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICoin 
{
    public int Coins { get; }
    void UpdateAction(UnityAction OnUpdate);
}
