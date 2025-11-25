using UnityEngine;

public class CubeRainSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private Transform _spawnAreaCenter;

    [Header("Spawn Area (XZ size)")]
    [SerializeField] private Vector3 _spawnAreaSize = new Vector3(10f, 0f, 10f);

    [Header("Spawn Timing")]
    [SerializeField] private float _spawnInterval = 0.5f;

    private float _timeSinceLastSpawn;

    private void Awake()
    {
        if (_cubePool == null)
            Debug.LogError("CubeRainSpawner: CubePool не назначен!");

        if (_spawnAreaCenter == null)
            Debug.LogError("CubeRainSpawner: SpawnAreaCenter не назначен!");
    }

    private void Update()
    {
        if (_cubePool == null || _spawnAreaCenter == null)
            return;

        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= _spawnInterval)
        {
            SpawnCube();
            _timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnCube()
    {
        Vector3 spawnPos = GetRandomPointInArea();
        _cubePool.GetFromPool(spawnPos);
    }

    private Vector3 GetRandomPointInArea()
    {
        float spawnRadiusX = _spawnAreaSize.x * 0.5f;
        float spawnRadiusZ = _spawnAreaSize.z * 0.5f;

        float offsetX = Random.Range(-spawnRadiusX, spawnRadiusX);
        float offsetZ = Random.Range(-spawnRadiusZ, spawnRadiusZ);

        Vector3 center = _spawnAreaCenter.position;

        return new Vector3(center.x + offsetX, center.y, center.z + offsetZ);
    }
}