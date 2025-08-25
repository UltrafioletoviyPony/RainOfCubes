using UnityEngine;

public class InvisableColliders : MonoBehaviour
{
    private void Awake()
    {
        float position = 5f;
        float size = 10f;
        float depth = 0.1f;
        float align = depth * .5f;

        CreateCollider(new Vector3(-position + -align, position, 0f), new Vector3(depth, size, size));
        CreateCollider(new Vector3(position + align, position, 0f), new Vector3(depth, size, size));
        CreateCollider(new Vector3(0f, position, position + align), new Vector3(size, size, depth));
        CreateCollider(new Vector3(0f, position, -position + -align), new Vector3(size, size, depth));
        CreateCollider(new Vector3(0f, position * 2 + align, 0f), new Vector3(size, depth, size));
    }

    private void CreateCollider(Vector3 center, Vector3 size)
    {
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.center = center;
        boxCollider.size = size;
    }
}