using UnityEngine;

public class StationUpgradesScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;

    private void OnDisable()
    {
        mainScreen.SetActive(!mainScreen.activeSelf);
    }

    public void VisualizeButton()
    {
        mainScreen.SetActive(!mainScreen.activeSelf);
    }
}
