     using UnityEngine;
     using System;

     public class PlayerHealth : MonoBehaviour
     {
         [Header("Health Settings")]
         public int maxHealth = 100;
         public int currentHealth { get; private set; }

         [Header("Damage Feedback")]
         public GameObject damageEffectPrefab;
         public float invulnerableDuration = 0.5f;

         private bool isInvulnerable = false;
         private Animator animator;

         public event Action<int> OnDamageTaken;
         public event Action OnPlayerDeath;

         void Awake()
         {
             animator = GetComponent<Animator>();
         }

         void Start()
         {
             currentHealth = maxHealth;
         }

         public void TakeDamage(int damage)
         {
             if (isInvulnerable || currentHealth <= 0) return;

             currentHealth -= damage;

             if (OnDamageTaken != null)
                 OnDamageTaken(damage);

             Debug.Log("Player kena damage: " + damage + " | HP: " + currentHealth + "/" + maxHealth);

             if (damageEffectPrefab != null)
             {
                 Instantiate(damageEffectPrefab, transform.position + Vector3.up, Quaternion.identity);
             }

             StartCoroutine(InvulnerableCoroutine());

             if (currentHealth <= 0)
             {
                 Die();
             }
         }

         System.Collections.IEnumerator InvulnerableCoroutine()
         {
             isInvulnerable = true;
             yield return new WaitForSeconds(invulnerableDuration);
             isInvulnerable = false;
         }

         void Die()
         {
             Debug.Log("PLAYER MATI!");

             if (OnPlayerDeath != null)
                 OnPlayerDeath();

             if (animator != null)
             {
                 animator.SetTrigger("IsDying");
             }

             // Disable movement
             UnityEngine.AI.NavMeshAgent nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
             if (nav != null) nav.isStopped = true;

             CharController controller = GetComponent<CharController>();
             if (controller != null) controller.enabled = false;

             // Restart setelah 3 detik
             Invoke(nameof(RestartLevel), 3f);
         }

         void RestartLevel()
         {
             UnityEngine.SceneManagement.SceneManager.LoadScene(
                 UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
             );
         }

         public void Heal(int amount)
         {
             if (currentHealth <= 0) return;
             currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
         }

         public int GetCurrentHealth()
         {
             return currentHealth;
         }
     }
