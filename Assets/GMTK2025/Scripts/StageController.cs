using Reflex.Attributes;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Inject]
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager.Reset();
    }
}
