using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private float fireRate = 1.0f;
    [SerializeField]
    private float bulletSpeed = 1000f;
    [SerializeField]
    private float laserRange = 5.0f;
    [SerializeField]
    private Material laserMaterial;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform muzzleTransform;

    private InputComponent inputComponent;
    private LineRenderer laser;

    private void Start()
    {
        inputComponent = GetComponentInParent<InputComponent>();
        laser = gameObject.AddComponent<LineRenderer>();

        laser.enabled = false;
        laser.alignment = LineAlignment.View;
        laser.startWidth = 0.05f;
        laser.endWidth = 0.05f;
        laser.startColor = Color.darkRed;
        laser.endColor = Color.red;
        laser.loop = true;
        laser.positionCount = 2;
        laser.material = laserMaterial;

        inputComponent.OnAimingStarted.AddListener(ShowLaser);
        inputComponent.OnAimingFinished.AddListener(HideLaser);
    }

    private void ShowLaser()
    {
        laser.enabled = true;
    }

    private void HideLaser()
    {
        laser.enabled = false;
    }

    private void OnDestroy()
    {
        inputComponent.OnAimingStarted.RemoveListener(ShowLaser);
        inputComponent.OnAimingFinished.RemoveListener(HideLaser);
    }

    private void Update()
    {
        if (laser.enabled)
        {
            laser.SetPosition(0, muzzleTransform.position);

            if (Physics.Linecast(muzzleTransform.position, muzzleTransform.up * laserRange, out RaycastHit hit, LayerMask.GetMask("Default")))
            {
                laser.SetPosition(1, muzzleTransform.position + hit.point);
            }

            laser.SetPosition(1, muzzleTransform.position + muzzleTransform.up * laserRange);
        }
    }

    public void Shoot(Vector3 direction)
    {
        Projectile bullet = Instantiate(projectilePrefab, muzzleTransform.position, Quaternion.identity).GetComponent<Projectile>();
        bullet.GetProjectileRigidbody().AddForce(direction * bulletSpeed);
    }

    public void SetProjectilePrefab(GameObject prefab)
    {
        projectilePrefab = prefab;
    }

    public float GetFireRate()
    {
        return fireRate;
    }
}
