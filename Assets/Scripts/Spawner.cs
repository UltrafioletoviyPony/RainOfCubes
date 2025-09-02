using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Cube> _pool;
    private GameObject _cubesParent;
    private float _repeatRate = 1.0f;
    private float _offsetPossition = 3;
    private Coroutine _coroutine;


    private void Awake()
    {
        _cubesParent = new GameObject(name: "Cubes");

        _pool = new ObjectPool<Cube>(
            createFunc: () => CreateCube(),
            actionOnGet: (cube) => GetCube(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => DestroyCube(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(nameof(SpawnRepeating));
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab, _cubesParent.transform);
        cube.Release += ReleaseCube;
        return cube;
    }

    private void GetCube(Cube cube)
    {
        cube.gameObject.transform.position = CreateRandomPosition();
        cube.gameObject.SetActive(true);
    }

    private void DestroyCube(Cube cube)
    {
        cube.Release -= ReleaseCube;
        Destroy(cube.gameObject);
    }

    private void GetCubes()
    {
        int cubeCount = 0;

        if (_pool.CountAll == 0)
            cubeCount = _poolCapacity;
        else if (_pool.CountInactive > 0)
            cubeCount = _pool.CountInactive;

        for (int i = 0; i < cubeCount; i++)
            _pool.Get();
    }

    private void ReleaseCube(Cube cube) =>
        _pool.Release(cube);

    private Vector3 CreateRandomPosition()
    {
        float randomPositionX = UnityEngine.Random.Range(-_offsetPossition, _offsetPossition + 1);
        float randomPositionZ = UnityEngine.Random.Range(-_offsetPossition, _offsetPossition + 1);

        Vector3 randomPosition = new Vector3(
                                    randomPositionX,
                                    transform.position.y,
                                    randomPositionZ
                                    );

        return randomPosition;
    }

    private IEnumerator SpawnRepeating()
    {
        bool isRuned = true;

        while (isRuned)
        {
            GetCubes();
            yield return new WaitForSeconds(_repeatRate);
        }
    }
}