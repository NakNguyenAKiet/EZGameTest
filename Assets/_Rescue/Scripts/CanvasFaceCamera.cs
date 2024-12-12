using UnityEngine;

public class CanvasFaceCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
    }
}
