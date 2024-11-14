using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader : MonoBehaviour
{
    [Inject] private ManagerData _data;
    [SerializeField] private GameObject _sound;

    AsyncOperation _asyncOp;

    private void Start()
    {
        LoadScene();
    }

    public void OnLoadNextScene()
    {
        Instantiate(_sound, _data.transform, false);
        _asyncOp.allowSceneActivation = true;
    }

    public async void LoadScene()
    {
        int sceneIndex = _data.IsTutor ? SceneManager.GetActiveScene().buildIndex + 1 : SceneManager.GetActiveScene().buildIndex + 2;
        Debug.Log($"{sceneIndex} {SceneManager.GetActiveScene().buildIndex} {_data.IsTutor}");
        _asyncOp = SceneManager.LoadSceneAsync(sceneIndex);
        _asyncOp.allowSceneActivation = false;

        do
        {
            await Task.Delay(100);
        } while (_asyncOp.progress < 0.9f);
        await Task.Delay(500);

        //_asyncOp.allowSceneActivation = true;
    }

}
