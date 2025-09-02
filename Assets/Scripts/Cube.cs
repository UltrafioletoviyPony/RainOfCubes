using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody), typeof(MeshRenderer))]
[RequireComponent(typeof(ColorChanger))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private ColorChanger _colorChanger;

    private Coroutine _coroutine;
    private int _minTime = 2;
    private int _maxTime = 5;
    private bool _isCollided;

    public event Action<Cube> Release;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.linearVelocity = Vector3.zero;
    }

    private void Start() =>
        ResetToDefault();

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollided == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            _isCollided = true;
            _colorChanger.SetColor(_renderer, Color.red);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(CountdownToRelease());
        }
    }

    private void ResetToDefault()
    {
        _colorChanger.SetColor(_renderer, Color.green);
        _isCollided = false;
    }

    private IEnumerator CountdownToRelease()
    {
        float elapsedTime = 0f;
        float coundounTime = UnityEngine.Random.Range(_minTime, _maxTime + 1);

        while (elapsedTime < coundounTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ResetToDefault();
        Release?.Invoke(this);
    }
}