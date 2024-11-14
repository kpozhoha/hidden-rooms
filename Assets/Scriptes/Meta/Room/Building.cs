using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public UnityEvent OnRevertView = new();

    public Renderer MainRenderer;
    public Vector2Int Size = Vector2Int.one;
    [SerializeField] public Vector2Int StartPlace;
    [SerializeField] private List<Renderer> MainRenderers = new();

    private ItemData _data;
    private Transform _container;

    public Transform Container { get => _container; set => _container = value; }

    private void Start()
    {
        if(gameObject.TryGetComponent<Item>(out var item))
        {
            _data = item.Data;
        }

        MainRenderers = gameObject.GetComponentsInChildren<Renderer>().ToList();
    }

    private void OnDisable()
    {
        OnRevertView.RemoveAllListeners();
    }

    public void SetTransparent(bool available)
    {
        if (available)
        {
            ChangeColorMesh(Color.green);
        }
        else
        {
            ChangeColorMesh(Color.red);
        }
    }

    public void SetNormal()
    {
        ChangeColorMesh(Color.white);
    }

    public void SetNormal(Vector2Int place)
    {
        ChangeColorMesh(Color.white);
        StartPlace = place;
    }

    public void DropBuilding()
    {
        //OnRevertView?.Invoke();
        if(_data != null)
        {
            InventoryController.OnRevertView?.Invoke(_data);
        }
        Destroy(gameObject);
    }

    private void ChangeColorMesh(Color color)
    {
        MainRenderers.ForEach(r =>
        {
            r.material.color = color;
        });
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if ((x + y) % 2 == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                else Gizmos.color = new Color(1f, 0.68f, 0f, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
}