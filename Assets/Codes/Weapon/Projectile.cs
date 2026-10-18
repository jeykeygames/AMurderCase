using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private float damage = 20f;
    [SerializeField]
    private bool simulateGravity = false;
    [SerializeField]
    private AudioClip projectileHitSound;
    [SerializeField]
    private AudioSource audioSource;

    private Rigidbody projectileRigidbody;

    private void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();

        projectileRigidbody.useGravity = simulateGravity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Damageable>(out Damageable damageable))
        {
            damageable.DoDamage(damage);
            audioSource.clip = projectileHitSound;
            audioSource.Play();
        }

        Destroy(gameObject);
    }

    public void OverrideProjectileDamage(float damage = 20f)
    {
        this.damage = Mathf.Abs(damage);
    }

    public Rigidbody GetProjectileRigidbody()
    {
        return projectileRigidbody;
    }
}
