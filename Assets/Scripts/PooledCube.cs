using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class PooledCube : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color _fallingColor = Color.white;  
    [SerializeField] private Color _touchedColor = Color.red;

    [Header("Lifetime (seconds)")]
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private CubePool _pool;

    private bool _lifetimeStarted;
    private float _remainingLifetime;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ResetState();
    }

    private void Update()
    {
        if (_lifetimeStarted == false)
            return;

        _remainingLifetime -= Time.deltaTime;

        if (_remainingLifetime <= 0f)
        {
            ReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_lifetimeStarted)
            return;

        if (collision.collider.TryGetComponent<PlatformSurface>(out _))
        {
            StartLifetime();
        }
    }

    public void AssignPool(CubePool pool)
    {
        _pool = pool;
    }

    private void ResetState()
    {
        _lifetimeStarted = false;
        _remainingLifetime = 0f;

        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        if (_renderer != null)
            _renderer.material.color = _fallingColor;
    }

    private void StartLifetime()
    {
        _lifetimeStarted = true;
        _remainingLifetime = Random.Range(_minLifetime, _maxLifetime);

        if (_renderer != null)
            _renderer.material.color = _touchedColor;
    }

    private void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.ReturnToPool(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}