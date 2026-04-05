using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    private bool isDead = false;
    
    [HideInInspector] public bool IsDying { get; private set; }

    public GameObject hitEffectPrefab;
    private Animator animator;

    public event Action<int> OnDamageTaken;
    public event Action OnDeath;

    void Awake()
    {
        animator = GetComponent<Animator>();
        IsDying = false;
    }

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + " HP Awal: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead || IsDying) return;

        currentHealth -= damage;

        OnDamageTaken?.Invoke(damage);

        Debug.Log(gameObject.name + " kena damage " + damage);
        Debug.Log("Sisa HP: " + currentHealth + " / " + maxHealth);

        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                hitEffectPrefab,
                transform.position + Vector3.up * 0.5f,
                Quaternion.identity
            );

            Destroy(effect, 1f);
        }

        transform.localScale = new Vector3(1.2f, 0.8f, 1.2f);
        Invoke(nameof(ResetScale), 0.1f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void ResetScale()
    {
        transform.localScale = Vector3.one;
    }

    void Die()
    {
        if (IsDying) return;
        
        IsDying = true;
        isDead = true;
        OnDeath?.Invoke();

        if (animator != null)
        {
            animator.SetBool("IsDying", true);
            animator.SetLayerWeight(1, 0);
        }
        else
        {
            Debug.LogWarning("[" + gameObject.name + "] Animator tidak ditemukan! Death animation tidak bisa play.");
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        Destroy(gameObject, 3f);
        Debug.Log(gameObject.name + " mati!");
    }
}
