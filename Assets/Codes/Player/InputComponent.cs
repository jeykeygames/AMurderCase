using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputComponent : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnShoot;

    [Space()]
    [Header("Modifiers")]
    [SerializeField]
    private float movementSpeed = 5.0f;
    [SerializeField]
    private float rotationSmoothness = 10.0f;
    [Space()]
    [Header("Fields")]
    [SerializeField]
    private Camera currentFixedCamera;
    [SerializeField]
    private Transform playerBody;

    private FixedCameraPlayer player;
    private CharacterController playerController;
    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction interactAction;
    private bool isMoving = false;
    private bool isAiming = false;
    private bool isShooting = false;
    private bool isInputLocked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GetComponent<FixedCameraPlayer>();
        playerController = GetComponent<CharacterController>();
        FindAnyObjectByType<CameraManager>().OnCameraChanged.AddListener(OnCameraChanged);

        moveAction = InputSystem.actions.FindAction("Move");
        aimAction = InputSystem.actions.FindAction("Aim");
        interactAction = InputSystem.actions.FindAction("Interact");

        moveAction.started += OnMoveStarted;
        moveAction.canceled += OnMoveCanceled;

        aimAction.started += OnAimStarted;
        aimAction.started += OnAimCanceled;

        interactAction.started += OnInteractStarted;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isInputLocked)
        {
            MovePlayer();
            Aim();
        }
    }

    private void OnInteractStarted(InputAction.CallbackContext obj)
    {
        if (isAiming)
        {
            OnShoot?.Invoke();
        }
        else
        {
            // interact
        }
    }

    private void OnCameraChanged(Camera camera)
    {
        if (camera)
        {
            currentFixedCamera = camera;
        }
    }

    private void OnAimCanceled(InputAction.CallbackContext context)
    {
        isAiming = false;
    }

    private void OnAimStarted(InputAction.CallbackContext context)
    {
        isAiming = true;
        isMoving = false;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (isAiming)
            return;

        isMoving = false;
    }

    private void OnMoveStarted(InputAction.CallbackContext obj)
    {
        if (isAiming)
            return;

        isMoving = true;
    }

    private void Aim()
    {
        if (isAiming)
        {

        }
    }

    private void MovePlayer()
    {
        if (isMoving)
        {
            Vector2 input = moveAction.ReadValue<Vector2>().normalized;
            FindRealForwardDirection(currentFixedCamera.transform, out Vector3 camForward, out Vector3 camRight);
            Vector3 movementDir = (camForward * input.y + camRight * input.x) * movementSpeed * Time.deltaTime;

            if (movementDir != Vector3.zero)
            {
                playerBody.rotation = Quaternion.Lerp(playerBody.rotation, RotateTowardsMovementDir(movementDir), Time.deltaTime * rotationSmoothness);
            }

            playerController.Move(movementDir);
        }
    }

    private void FindRealForwardDirection(Transform transform, out Vector3 forward, out Vector3 right)
    {
        forward = transform.forward;
        right = transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
    }

    private Quaternion RotateTowardsMovementDir(Vector3 forwardDirection)
    {
        return Quaternion.LookRotation(forwardDirection, Vector3.up);
    }

    private void OnDestroy()
    {
        moveAction.started -= OnMoveStarted;
        moveAction.canceled -= OnMoveCanceled;

        aimAction.started -= OnAimStarted;

        interactAction.started -= OnInteractStarted;

        FindAnyObjectByType<CameraManager>().OnCameraChanged.RemoveListener(OnCameraChanged);
    }

    public void SetActiveCamera(Camera camera)
    {
        currentFixedCamera = camera;
    }
}
