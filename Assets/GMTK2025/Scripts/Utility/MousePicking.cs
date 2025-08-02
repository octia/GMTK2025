using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousePicking
{
    private static readonly RaycastHit[] hits = new RaycastHit[100];

    public static bool PickObjectFromLayer<T>(
        Func<T, bool> acceptanceFilter,
        LayerMask mask,
        out T hitT,
        out Vector3 hitPosition,
        Camera cam,
        QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal
    )
        where T : MonoBehaviour
    {
        hitT = null;
        hitPosition = Vector3.zero;

        var ray = cam.ScreenPointToRay(Mouse.current.position.value);

        for (int attempt = 0; attempt <= 3; attempt++)
        {
            T closestT = null;
            float closestDistance = Mathf.Infinity;
            var hitCount = Physics.SphereCastNonAlloc(
                ray,
                3 * (attempt / 3.0f),
                hits,
                Mathf.Infinity,
                mask,
                triggerInteraction
            );
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].distance >= closestDistance)
                {
                    continue;
                }
                hitT = hits[i].transform.GetComponentInParent<T>();
                if (!acceptanceFilter(hitT))
                {
                    continue;
                }
                closestT = hitT;
                closestDistance = hits[i].distance;
                hitPosition = hits[i].point;
            }
            if (closestT)
            {
                hitT = closestT;
                return true;
            }
        }

        return false;
    }
}
