using System.Collections.Generic;
using UnityEngine;

public class CubePool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private RainingObject _rainingObjectPrefab;
    [SerializeField] private int _initialPoolSize = 20;
    [SerializeField] private bool _canExpand = true;

    private readonly Queue<RainingObject> _inactiveObjects = new Queue<RainingObject>();

    private void Awake()
    {
        if (_rainingObjectPrefab == null)
        {
            Debug.LogError("CubePool: RainingObject prefab не назначен!");
            return;
        }

        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateAndStoreObject();
        }
    }

    public RainingObject GetFromPool(Vector3 position)
    {
        if (_inactiveObjects.Count == 0)
        {
            if (_canExpand)
            {
                CreateAndStoreObject();
            }
            else
            {
                return null;
            }
        }

        RainingObject pooledObject = _inactiveObjects.Dequeue();
        pooledObject.transform.position = position;
        pooledObject.transform.rotation = Quaternion.identity;
        pooledObject.gameObject.SetActive(true);
        return pooledObject;
    }

    private RainingObject CreateAndStoreObject()
    {
        RainingObject rainingObject = Instantiate(_rainingObjectPrefab, transform);
        rainingObject.gameObject.SetActive(false);
        rainingObject.LifeEnded += HandleObjectLifeEnded;
        _inactiveObjects.Enqueue(rainingObject);
        return rainingObject;
    }

    private void HandleObjectLifeEnded(RainingObject pooledObject)
    {
        ReturnToPool(pooledObject);
    }

    private void ReturnToPool(RainingObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        _inactiveObjects.Enqueue(pooledObject);
    }
}