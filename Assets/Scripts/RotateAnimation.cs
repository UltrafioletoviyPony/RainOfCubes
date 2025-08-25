using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 5;
    [SerializeField] private Vector3 _rotateAxies = new Vector3(0, 1, 0);

    private Vector3 _centerPosition = Vector3.zero;

    void Update() =>
        transform.RotateAround(_centerPosition, _rotateAxies, _rotateSpeed * Time.deltaTime);
}
