using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Damageable))]
public class HealthSystem : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnHealthFinished;
    public UnityEvent OnHealthIncreased;
    public UnityEvent OnHealthDecreased;

    [Space()]
    [Header("Properties")]
    [SerializeField]
    private float maxHealth = 100f;
    
    private float health = 100f;


    private float ClampHealthValue()
    {
        return Mathf.Clamp(health, 0f, maxHealth);
    }

    private bool IsDead()
    {
        if (health <= 0f)
        {
            return true;
        }

        return false;
    }

    public void Damage(float damage)
    {
        health -= damage;
        health = ClampHealthValue();
        OnHealthDecreased?.Invoke();
        
        if (IsDead())
        {
            OnHealthFinished?.Invoke();
        }
    }

    public void Heal(float value)
    {
        health += value;
        health = ClampHealthValue();
        OnHealthIncreased?.Invoke();
    }
}
