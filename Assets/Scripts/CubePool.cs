using System.Collections.Generic;
using UnityEngine;

public class CubePool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private PooledCube _cubePrefab;
    [SerializeField] private int _initialPoolSize = 20;
    [SerializeField] private bool _canExpand = true;

    private readonly Queue<PooledCube> _inactiveCubes = new Queue<PooledCube>();

    private void Awake()
    {
        if (_cubePrefab == null)
        {
            Debug.LogError("CubePool: префаб PooledCube не назначен!");
            return;
        }

        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateAndStoreCube();
        }
    }

    private PooledCube CreateAndStoreCube()
    {
        PooledCube cube = Instantiate(_cubePrefab, transform);
        cube.AssignPool(this);
        cube.gameObject.SetActive(false);
        _inactiveCubes.Enqueue(cube);

        return cube;
    }

    public PooledCube GetFromPool(Vector3 position)
    {
        if (_inactiveCubes.Count == 0)
        {
            if (_canExpand)
            {
                CreateAndStoreCube();
            }
            else
            {
                return null;
            }
        }

        PooledCube cube = _inactiveCubes.Dequeue();
        cube.transform.position = position;
        cube.gameObject.SetActive(true);
        return cube;
    }

    public void ReturnToPool(PooledCube cube)
    {
        cube.gameObject.SetActive(false);
        _inactiveCubes.Enqueue(cube);
    }
}