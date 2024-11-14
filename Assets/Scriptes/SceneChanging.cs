using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanging : MonoBehaviour
{
    //private void Start()
    //{
    //    GetComponent<Button>().onClick.AddListener(LoadScene);
    //}

    //private void OnDisable()
    //{
    //    GetComponent<Button>().onClick.RemoveListener(LoadScene);
    //}

    public void LoadScene(int index)
    {
        Debug.Log("Change scene");
        //GlobalEvents.UpdateTutor?.Invoke();
        SceneManager.LoadScene(index);
    }

    private void LoadScene()
    {
        GlobalEvents.UpdateTutor?.Invoke();
        Debug.Log("Change scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
