using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopHand : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _hand;
    
    public void DisableLoop()
    {
        _hand.SetActive(false);
        gameObject.SetActive(false);
    }
}
