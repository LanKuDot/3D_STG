using System.Collections;
using UnityEngine;

/// <summary>
/// Flash the color of the material that renderer has
/// </summary>
public class MaterialColorFlash : MonoBehaviour
{
    [SerializeField] [ShowOnly]
    private Renderer _renderer = null;
    [SerializeField]
    [Tooltip("The target color that will be flashed to")]
    private Color _targetColor = Color.white;
    [SerializeField]
    [Tooltip("The curve defining the progress of color changing. " +
             "The value in range [0,1] means [origColor, targetColor]")]
    private AnimationCurve _flashingCurve = new AnimationCurve(
            new Keyframe(0.0f, 0.0f, 0.0f, 113.0f),
            new Keyframe(0.02f, 1.0f),
            new Keyframe(0.1f, 0.0f, -31.0f, 0.0f));

    private Color _origColor;
    private float _flashTime = 0.1f;
    private readonly int _colorID = Shader.PropertyToID("_Color");

    private void Reset()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _origColor = _renderer.material.GetColor(_colorID);
        _flashTime = _flashingCurve[_flashingCurve.length - 1].time;
    }

    /// <summary>
    /// Do a color flashing
    /// </summary>
    public void Flash()
    {
        StartCoroutine(ColorFlashing());
    }

    private IEnumerator ColorFlashing()
    {
        var timePassed = 0.0f;
        var targetMaterial = _renderer.material;

        while ((timePassed += Time.deltaTime) < _flashTime) {
            targetMaterial.SetColor(
                _colorID,
                Color.Lerp(
                    _origColor, _targetColor, _flashingCurve.Evaluate(timePassed)));
            yield return null;
        }

        targetMaterial.SetColor(_colorID, _origColor);
    }
}
