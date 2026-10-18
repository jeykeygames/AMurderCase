using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private float fireRate = 1.0f;
    [SerializeField]
    private float bulletSpeed = 1000f;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform muzzleTransform;

    public void Shoot()
    {
        Projectile bullet = Instantiate(projectilePrefab, muzzleTransform.position, Quaternion.identity).GetComponent<Projectile>();
        bullet.GetProjectileRigidbody().AddForce(muzzleTransform.forward * bulletSpeed);
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
