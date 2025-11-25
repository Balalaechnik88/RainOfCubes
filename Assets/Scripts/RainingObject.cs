using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ColorChanger))]
public class RainingObject : MonoBehaviour
{
    [Header("Lifetime (seconds)")]
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;

    private Rigidbody _rigidbody;
    private ColorChanger _colorChanger;

    private bool _lifetimeStarted;
    private float _remainingLifetime;

    public event Action<RainingObject> LifeEnded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colorChanger = GetComponent<ColorChanger>();
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
            _lifetimeStarted = false;
            LifeEnded?.Invoke(this); 
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

    private void ResetState()
    {
        _lifetimeStarted = false;
        _remainingLifetime = 0f;

        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        _colorChanger?.ApplyFallingColor();
    }

    private void StartLifetime()
    {
        _lifetimeStarted = true;
        _remainingLifetime = UnityEngine.Random.Range(_minLifetime, _maxLifetime);
        _colorChanger?.ApplyTouchedColor();
    }
}