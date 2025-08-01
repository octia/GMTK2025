using Reflex.Attributes;
using UnityEngine;

public class ShipCollisionManager
{
    [Inject]
    private SceneManager sceneManager;

    public void CollisionHappened(Ship ship1, Ship ship2, Vector3 collisionPoint)
    {
        // Destroy ships, instantiate explosion from here.
        // Ensure duplicates are ignored.

        sceneManager.LoadGameplay();

        if (ship1)
        {
            Object.Destroy(ship1.gameObject);
        }
        if (ship2)
        {
            Object.Destroy(ship2.gameObject);
        }
    }
}
