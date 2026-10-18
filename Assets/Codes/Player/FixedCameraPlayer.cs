using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class FixedCameraPlayer : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField]
    private Weapon weapon;

    private HealthSystem healthSystem;
    private InputComponent inputComponent;
    private Animator animator;
    private bool isMoving = false;
    private bool isAiming = false;
    private bool canShoot = true;
    private bool isShooting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        animator = GetComponentInChildren<Animator>();
        inputComponent = GetComponent<InputComponent>();

        inputComponent.OnShootingStarted.AddListener(OnShootingStarted);
        inputComponent.OnShootingFinished.AddListener(OnShootingFinished);
        inputComponent.OnMovementStarted.AddListener(OnMoveStarted);
        inputComponent.OnMovementFinished.AddListener(OnMoveFinished);
        inputComponent.OnAimingStarted.AddListener(OnAimStarted);
        inputComponent.OnAimingFinished.AddListener(OnAimFinished);

        healthSystem.OnHealthFinished.AddListener(Die);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateMachine();

        if (isShooting)
        {
            Shoot();
        }

        if (!canShoot)
        {
            StartCoroutine("FireDelay", weapon.GetFireRate());
        }
    }

    private void OnDestroy()
    {
        healthSystem.OnHealthFinished.RemoveListener(Die);

        inputComponent.OnShootingStarted.RemoveListener(OnShootingStarted);
        inputComponent.OnShootingFinished.RemoveListener(OnShootingFinished);
        inputComponent.OnMovementStarted.RemoveListener(OnMoveStarted);
        inputComponent.OnMovementFinished.RemoveListener(OnMoveFinished);
        inputComponent.OnAimingStarted.RemoveListener(OnAimStarted);
        inputComponent.OnAimingFinished.RemoveListener(OnAimFinished);
    }

    private void AnimatorStateMachine()
    {
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsAiming", isAiming);
    }

    private void OnMoveStarted()
    {
        isMoving = true;
    }

    private void OnMoveFinished()
    {
        isMoving = false;
    }

    private void OnAimStarted()
    {
        isAiming = true;
    }

    private void OnAimFinished()
    {
        isAiming = false;
    }

    private void Die()
    {
        Debug.Log("Player is dead!");
    }

    private IEnumerator FireDelay(float fireRate)
    {
        float fireDeltaTime = 0f;

        while (fireDeltaTime <= fireRate)
        {
            fireDeltaTime += Time.deltaTime;
            yield return null;
        }

        canShoot = true;
    }

    private void OnShootingStarted()
    {
        isShooting = true;
    }

    private void OnShootingFinished()
    {
        isShooting = false;
    }

    private void Shoot()
    {
        if (weapon && canShoot)
        {
            weapon.Shoot();
            animator.SetTrigger("IsShooting");
            canShoot = false;
        }
    }
}
