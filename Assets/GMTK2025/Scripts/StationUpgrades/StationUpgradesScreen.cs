using UnityEngine;

public class StationUpgradesScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject mainScreen;

    private void OnDisable()
    {
        mainScreen.SetActive(false);
    }

    public void VisualizeButton()
    {
        mainScreen.SetActive(!mainScreen.activeSelf);
    }
}
