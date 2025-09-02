using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void SetColor(Renderer renderer, Color color) =>
            renderer.material.color = color;
}