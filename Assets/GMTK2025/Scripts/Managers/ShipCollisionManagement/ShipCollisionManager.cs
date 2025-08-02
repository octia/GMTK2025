using PrimeTween;
using Reflex.Attributes;
using UnityEngine;

public class ShipCollisionManager
{
    [Inject]
    private ShipCollisionManagerConfig config;

    [Inject]
    private LifeManager lifeManager;

    [Inject]
    private SoundManager soundManager;

    public void CollisionHappened(Ship ship1, Ship ship2, Vector3 collisionPoint)
    {
        // Destroy ships, instantiate explosion from here.
        // Ensure duplicates are ignored.

        if (ship1.enabled && ship2.enabled)
        {
            Ship[] ships = new Ship[] { ship1, ship2 };
            for (int i = 0; i < ships.Length; i++)
            {
                var shipToExplode = ships[i];
                shipToExplode.enabled = false;
                var rigidbody = shipToExplode.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                rigidbody.detectCollisions = false;
                rigidbody.linearVelocity =
                    (shipToExplode.transform.position - collisionPoint).normalized * 3;
                rigidbody.linearVelocity += (i == 0 ? Vector3.up : Vector3.down) * 2;
                rigidbody.angularVelocity = Random.onUnitSphere * 10;
                Sequence
                    .Create()
                    .ChainDelay(2.0f)
                    .Chain(Tween.Scale(shipToExplode.transform, 0.0f, 1.0f))
                    .OnComplete(shipToExplode.DestroyShip);
            }
            GameObject.Instantiate(config.explosionPrefab, collisionPoint, Quaternion.identity);
            soundManager.PlayOneShot(soundManager.soundManagerConfig.sfxExplosionPath);

            lifeManager.LoseLife();
        }
    }
}
