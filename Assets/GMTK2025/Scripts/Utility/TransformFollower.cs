using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    [SerializeField] public Transform FollowTarget;

    private void Update()
    {
        transform.position = FollowTarget.position;
    }
}
