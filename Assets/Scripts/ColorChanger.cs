using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Color _fallingColor = Color.white; 
    [SerializeField] private Color _touchedColor = Color.red;  

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void ApplyFallingColor()
    {
        if (_renderer != null)
            _renderer.material.color = _fallingColor;
    }

    public void ApplyTouchedColor()
    {
        if (_renderer != null)
            _renderer.material.color = _touchedColor;
    }
}