using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BarIndicator : MonoBehaviour
{
    private Renderer renderer;

    private Material materialInstance;
    private int baseMapId = Shader.PropertyToID("_BaseMap");

    [SerializeField] private Gradient gradient;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        materialInstance = new Material(renderer.sharedMaterial);
        renderer.sharedMaterial = materialInstance;

        SetRatio(0);
    }

    public void SetRatio(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        materialInstance.SetTextureOffset(baseMapId, new Vector2((1 - ratio)*0.5f, 0));
        materialInstance.color = gradient.Evaluate(ratio);
    }
}
