using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody), typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    private Coroutine _coroutine;
    private int _minTime = 2;
    private int _maxTime = 5;
    private bool _isCollided;

    public event Action<Cube> Release;

    private void Start() =>
        ResetToDefault();

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollided == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            _isCollided = true;
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(CountdownToRelease());
        }
    }

    private void ResetToDefault()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        this.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        _isCollided = false;
    }

    private IEnumerator CountdownToRelease()
    {
        float elapsedTime = 0f;

        while (elapsedTime < UnityEngine.Random.Range(_minTime, _maxTime + 1))
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ResetToDefault();
        Release?.Invoke(this);
    }
}