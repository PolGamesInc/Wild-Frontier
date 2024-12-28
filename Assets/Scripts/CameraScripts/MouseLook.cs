using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform Player;

    public float Sensitivity = 100f;

    private float XRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float RotationX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        float RotationY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

        XRotation -= RotationY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
        Player.Rotate(Vector3.up * RotationX);
    }
}
