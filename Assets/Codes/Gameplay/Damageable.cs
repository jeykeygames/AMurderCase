using UnityEngine;

public class Damageable : MonoBehaviour
{
    private HealthSystem hostHealthSystem;

    private void Start()
    {
        hostHealthSystem = GetComponent<HealthSystem>();
    }

    public bool DoDamage(float damage)
    {
        if (hostHealthSystem)
        {
            hostHealthSystem.Damage(damage);
            return true;
        }

        return false;
    }
}
