using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    [SerializeField]
    private ushort index;

    private CameraManager cameraManager;

    private void Start()
    {
        cameraManager = FindAnyObjectByType<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            cameraManager.DiactivateCamera(cameraManager.GetActiveCameraIndex());
            cameraManager.ActivateCamera(index);
        }
    }
}
