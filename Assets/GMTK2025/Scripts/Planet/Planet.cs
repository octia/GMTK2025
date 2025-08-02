using Reflex.Attributes;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Inject] private TimerManager timerManager;
    [SerializeField] private PlanetConfig config;
    [SerializeField] private float rotationSpeed = 10;

    private void Start()
    {
        var planetProfile = config.GetPlanetProfile();
        var instance = Instantiate(planetProfile.prefab, transform);
        instance.GetComponentInChildren<Renderer>().sharedMaterial = planetProfile.material;
        var rotation = planetProfile.useRandomRotation ? Random.rotation : Quaternion.identity;
        instance.transform.SetLocalPositionAndRotation(transform.position, rotation);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * timerManager.TimeScale);
    }
}
