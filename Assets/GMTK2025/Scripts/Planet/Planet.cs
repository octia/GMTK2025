using Reflex.Attributes;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Inject]
    private TimerManager timerManager;

    [SerializeField]
    private PlanetConfig config;

    [SerializeField]
    private bool isPrimary = false;

    [SerializeField]
    private float rotationSpeed = 10;

    private static PlanetConfig.PlanetProfile mainPlanetProfile = default;

    private Vector3 rotationAxis;

    private void Awake()
    {
        rotationAxis = Random.onUnitSphere;
        if (isPrimary)
        {
            RandomizePlanet();
        }
    }

    private void OnEnable()
    {
        if (!isPrimary)
        {
            RandomizePlanet();
        }
    }

    private void RandomizePlanet()
    {
        PlanetConfig.PlanetProfile planetProfile;

        if (isPrimary)
        {
            planetProfile = config.GetPlanetProfile();
            mainPlanetProfile = planetProfile;
        }
        else
        {
            planetProfile = config.GetPlanetProfileExcept(mainPlanetProfile);
            transform.localScale = Vector3.one * 0.8f;
        }
        var instance = Instantiate(planetProfile.prefab, transform);

        instance.GetComponentInChildren<Renderer>().sharedMaterial = planetProfile.material;
        var rotation = planetProfile.useRandomRotation ? Random.rotation : Quaternion.identity;
        instance.transform.SetLocalPositionAndRotation(Vector3.zero, rotation);

        if (!isPrimary)
        {
            transform.localPosition = Random.onUnitSphere * 250f;
        }
    }

    private void Update()
    {
        transform.Rotate(
            rotationAxis,
            rotationSpeed * Time.deltaTime * timerManager.TimeScale,
            Space.Self
        );
    }
}
