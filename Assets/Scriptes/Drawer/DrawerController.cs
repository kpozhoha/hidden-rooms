using PaintIn3D;
using UnityEngine;
using Zenject;

public class DrawerController : MonoBehaviour
{
    [Inject] private ManagerData _managerData;

    [SerializeField] private GameObject _maskPrefab;
    [SerializeField] private ParticleSystem _effectsDrawer;

    [SerializeField] private GameObject _maskCanvas;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private TutorController _tutor;

    [SerializeField] private DragPitchYaw _dragPitchYaw;

    private GameObject _currentMask;
    private P3dChangeCounter _counter;
    private ParticleSystem effect;

    private void Awake()
    {
        _levelManager.OnStartDrawer = InitDrawer;
        InitDrawer();
    }

    public void InitDrawer()
    {
        _levelManager.enabled = false;
        _maskCanvas.SetActive(true);
        _currentMask = Instantiate(_maskPrefab, transform, false);
        _dragPitchYaw.SetDefaultPosition();
        _counter = _currentMask.GetComponent<P3dChangeCounter>();
        effect = Instantiate(_effectsDrawer, transform, false);
        if (_managerData.Level == 1)
        {
            var fit = effect.gameObject.AddComponent<FitMask>();
            fit.IsEndShow = true;
            fit.IsShowMask = true;
            fit.TutorType = TutorType.Drawer;
        }
    }

    private void Update()
    {
        if(_counter != null && _counter.Ratio >= 1)
        {
            DiactiveDrawer();
        }
    }


    private void DiactiveDrawer()
    {
        _levelManager.OnStartLevel(_tutor.UpdateTutor);
        effect.Play();
        _maskCanvas.SetActive(false);
        _counter = null;
        _dragPitchYaw.enabled = true;
        Camera.main.fieldOfView = 60f;
        Destroy(_currentMask);
        Destroy(effect.gameObject, 1f);
    }
}
