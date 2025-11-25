using System.Collections;
using UnityEngine;

public class CubeRainSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private Transform _spawnAreaCenter;

    [Header("Spawn Area (XZ size)")]
    [SerializeField] private Vector2 _spawnAreaSize = new Vector2(10f, 10f);

    [Header("Spawn Timing")]
    [SerializeField] private float _spawnInterval = 0.5f;

    private Coroutine _spawnCoroutine;

    private void OnEnable()
    {
        if (_spawnCoroutine == null)
            _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (_cubePool != null && _spawnAreaCenter != null)
            {
                Vector3 spawnPosition = GetRandomPointInArea();
                _cubePool.GetFromPool(spawnPosition);
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private Vector3 GetRandomPointInArea()
    {
        float spawnRadiusX = _spawnAreaSize.x * 0.5f;
        float spawnRadiusZ = _spawnAreaSize.y * 0.5f;

        float offsetX = Random.Range(-spawnRadiusX, spawnRadiusX);
        float offsetZ = Random.Range(-spawnRadiusZ, spawnRadiusZ);

        Vector3 center = _spawnAreaCenter.position;
        return new Vector3(center.x + offsetX, center.y, center.z + offsetZ);
    }
}