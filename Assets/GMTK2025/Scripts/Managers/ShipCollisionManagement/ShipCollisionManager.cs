using Reflex.Attributes;
using UnityEngine;

public class ShipCollisionManager
{
    [Inject]
    private LifeManager lifeManager;

    public void CollisionHappened(Ship ship1, Ship ship2, Vector3 collisionPoint)
    {
        // Destroy ships, instantiate explosion from here.
        // Ensure duplicates are ignored.

        if (!ship1.IsDestroyed && !ship2.IsDestroyed)
        {
            ship1.DestroyShip();
            ship2.DestroyShip();

            lifeManager.LoseLife();
        }
    }
}
