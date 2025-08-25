using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public event Action<GameObject, Collision, Coroutine> Collided;
    public event Action<GameObject> Changed;

    private Coroutine _coroutine;
    private float _coundounTime;
    private int _minTime = 2;
    private int _maxTime = 5;

    private void Start() =>
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;

    private void OnCollisionEnter(Collision collision) =>
        Collided?.Invoke(this.gameObject, collision, _coroutine);

    public void Change()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        _coroutine = StartCoroutine(CountdownToRelease());
    }

    private IEnumerator CountdownToRelease()
    {
        _coundounTime = CreateRnadomCountdownTime();
        float elapsedTime = 0f;

        while (elapsedTime < _coundounTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        StopCoroutine(_coroutine);
        _coroutine = null;
        Changed?.Invoke(this.gameObject);
    }

    private float CreateRnadomCountdownTime() =>
        UnityEngine.Random.Range(_minTime, _maxTime + 1);
}