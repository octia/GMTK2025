using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GMTK2025/Config/PlanetConfig", fileName = "PlanetConfig")]
public class PlanetConfig : ScriptableObject
{
    [SerializeField]
    private List<PlanetProfile> planetConfigurations;

    [System.Serializable]
    public struct PlanetProfile
    {
        public GameObject prefab;
        public Material material;
        public bool useRandomRotation;
    }

    public PlanetProfile GetPlanetProfile()
    {
        return planetConfigurations.RandomElement();
    }

    public PlanetProfile GetPlanetProfileExcept(PlanetProfile exception)
    {
        return planetConfigurations[
            planetConfigurations.RandomIndexDifferentFrom(planetConfigurations.IndexOf(exception))
        ];
    }
}
