using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Config/PlanetConfig", fileName = "PlanetConfig")]
public class PlanetConfig : ScriptableObject
{
    [SerializeField]
    private PlanetProfile[] planetConfigurations;

    [System.Serializable]
    public struct PlanetProfile
    {
        public GameObject prefab;
        public Material material;
        public bool useRandomRotation;
    }

    public PlanetProfile GetPlanetProfile()
    {
        return planetConfigurations[Random.Range(0, planetConfigurations.Length)];
    }
}
