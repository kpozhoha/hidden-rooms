using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Item : MonoBehaviour, IItem
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private float _upDistance = 10f;

    private UnityAction OnEnd;

    private Vector3 _startPos;
    private Tween _tween;

    public string Name => _itemData.NameItem;

    public ItemData Data => _itemData;

    private void Awake()
    {
        _startPos = transform.position;
    }

    public void OnFind()
    {
        _particleSystem.Play();

        int index = Random.Range(0, _particleSystems.Length);
        gameObject.GetComponent<Collider>().enabled = false;

        Debug.Log(_upDistance);
        _tween = transform
            .DOLocalMoveY(transform.localPosition.y + _upDistance, 1f)
            .SetEase(Ease.InOutSine)
            .OnKill(() =>
            {
                gameObject.SetActive(false);
                StartParticle(index);
                OnEnd?.Invoke();
            });
    }

    private void StartParticle(int index)
    {
        var particle = Instantiate(_particleSystems[index], transform.position + Vector3.up * .5f + Vector3.right * (-.5f) + Vector3.forward * .5f, Quaternion.identity);
        particle.transform.localScale = Vector3.one * 0.7f;
        Destroy(particle.gameObject, 1f);
    }

    public void OnWrong()
    {
        Debug.Log("wrong item"); // ToDo: реализовать функционал небольшой встряски 
        transform.position = _startPos;
        if(_tween != null)
            _tween.Kill();
        _tween = transform.DOShakePosition(1, 2f, 10, 90, false, true, ShakeRandomnessMode.Harmonic);
    }

    public void OnFind(UnityAction OnCheckLevel) // если конец уровня, вызывать метод концовки
    {
        _particleSystem.Play();

        int index = Random.Range(0, _particleSystems.Length);
        gameObject.GetComponent<Collider>().enabled = false;

        _tween = transform
            .DOLocalMoveY(transform.localPosition.y + _upDistance, 1f)
            .SetEase(Ease.InOutSine)
            .OnKill(() =>
            {
                gameObject.SetActive(false);
                StartParticle(index);
                OnCheckLevel?.Invoke();
            });
        //OnCheckLevel();
    }

    public void OnFind(UnityAction OnRemoveView, UnityAction OnCheckLevel = null)
    {
        _particleSystem.Play();

        int index = Random.Range(0, _particleSystems.Length);
        gameObject.GetComponent<Collider>().enabled = false;

        _tween = transform
            .DOLocalMoveY(transform.localPosition.y + _upDistance, 1f)
            .SetEase(Ease.InOutSine)
            .OnKill(() =>
            {
                gameObject.SetActive(false);
                StartParticle(index);
                OnRemoveView?.Invoke();
                OnCheckLevel?.Invoke();
            });
    }

    public void OnFind(UnityAction<string> OnRemoveView, UnityAction OnCheckLevel = null)
    {
        _particleSystem.Play();

        int index = Random.Range(0, _particleSystems.Length);
        gameObject.GetComponent<Collider>().enabled = false;

        _tween = transform
            .DOLocalMoveY(transform.localPosition.y + _upDistance, 1f)
            .SetEase(Ease.InOutSine)
            .OnKill(() =>
            {
                gameObject.SetActive(false);
                StartParticle(index);
                OnRemoveView?.Invoke(Name);
                OnCheckLevel?.Invoke();
            });
    }
}

