using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Vector3 CameraOffset;

    private void LateUpdate()
    {
        transform.position = Player.position + CameraOffset;
    }
}
