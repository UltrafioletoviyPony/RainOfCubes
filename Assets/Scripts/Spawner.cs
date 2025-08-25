using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<GameObject> _pool;
    private List<GameObject> _objects = new();
    private GameObject _cubesParent;

    private float _OffsetPossition = 3;


    private void Awake()
    {
        _cubesParent = new GameObject(name: "Cubes");

        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrefab, _cubesParent.transform),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        GetCubes();
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = CreateRandomPosition();
        obj.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void GetCubes()
    {
        for (int i = 0; i < _poolMaxSize; i++)
            _objects.Add(_pool.Get());
    }

    private void OnEnable()
    {
        foreach (GameObject obj in _objects)
        {
            obj.GetComponent<Cube>().Collided += Collide;
            obj.GetComponent<Cube>().Changed += Release;

        }
    }

    private void OnDisable()
    {
        foreach (GameObject obj in _objects)
        {
            if (obj != null)
            {
                obj.GetComponent<Cube>().Collided -= Collide;
                obj.GetComponent<Cube>().Changed -= Release;
            }
        }
    }

    private void Collide(GameObject cube, Collision collision, Coroutine coroutine)
    {
        if (collision.gameObject.TryGetComponent(out Wall wall) && coroutine == null)
            cube.GetComponent<Cube>().Change();
    }

    private void Release(GameObject cube)
    {
        _pool.Release(cube);
        _pool.Get();
    }

    private Vector3 CreateRandomPosition()
    {
        float randomPositionX = UnityEngine.Random.Range(-_OffsetPossition, _OffsetPossition + 1);
        float randomPositionZ = UnityEngine.Random.Range(-_OffsetPossition, _OffsetPossition + 1);

        Vector3 randomPosition = new Vector3(
                                    randomPositionX,
                                    transform.position.y,
                                    randomPositionZ
                                    );

        return randomPosition;
    }
}