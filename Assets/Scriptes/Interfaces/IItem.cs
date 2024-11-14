using UnityEngine;
using UnityEngine.Events;

public interface IItem
{
    ItemData Data { get; }
    string Name { get; }

    //void OnFind(UnityAction OnRemoveView = null, UnityAction OnCheckLevel = null);
    void OnFind(UnityAction<string> OnRemoveView = null, UnityAction OnCheckLevel = null);

    void OnWrong();
}
