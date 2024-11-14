using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void OnClickTutor()
    {
        GlobalEvents.UpdateTutor?.Invoke();
    }
}
