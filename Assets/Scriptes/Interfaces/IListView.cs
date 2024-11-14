using UnityEngine;

public interface IListView
{
    bool UpdateView(string name);
    void DeleteView();
    void DeleteView(string nameItem);


}