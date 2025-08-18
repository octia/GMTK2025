using UnityEngine;

public class RotateUI : MonoBehaviour
{
    [SerializeField]
    public Vector3 angularSpeed;

    private void Update() => transform.Rotate(angularSpeed * Time.deltaTime);
}
