using System;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<Camera> OnCameraChanged;

    [Space()]
    [Header("Cameras")]
    [SerializeField]
    private Camera[] fixedCameras;

    private Camera activeCamera;
    private ushort activeCameraIndex;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (Camera cam in fixedCameras)
        {
            cam.gameObject.SetActive(false);
        }

        fixedCameras[0].gameObject.SetActive(true);
        activeCamera = fixedCameras[0];
        activeCameraIndex = 0;
    }

    public bool ActivateCamera(ushort index)
    {
        if (index >= fixedCameras.Length)
            return false;

        fixedCameras[index].gameObject.SetActive(true);
        activeCamera = fixedCameras[index];
        activeCameraIndex = index;
        OnCameraChanged?.Invoke(activeCamera);
        return true;
    }

    public bool DiactivateCamera(ushort index)
    {
        if (index >= fixedCameras.Length)
            return false;

        fixedCameras[index].gameObject.SetActive(false);
        activeCamera = null;
        activeCameraIndex = 0;
        return true;
    }

    public ushort GetActiveCameraIndex()
    {
        return activeCameraIndex;
    }
}
