using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class Orbit : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1.0f;

    private Material material;
    private static int mainTextureId = Shader.PropertyToID("_BaseMap");

    private void Awake()
    {
        var renderer = GetComponent<MeshRenderer>();
        material = renderer.material;
    }

    void Update()
    {
        material.SetTextureOffset(mainTextureId, new Vector2(0, Time.time * scrollSpeed));
    }
}
