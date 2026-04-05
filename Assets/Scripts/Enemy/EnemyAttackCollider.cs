using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    [Header("Settings")]
    public int damage = 15;
    public LayerMask playerLayer;

    private SphereCollider col;
    private bool canDamage = false;

    void Awake()
    {
        col = GetComponent<SphereCollider>();
        if (col != null)
        {
            col.enabled = false;
            col.isTrigger = true;
        }
    }

    public void StartDamage()
    {
        canDamage = true;
        if (col != null)
            col.enabled = true;
    }

    public void EndDamage()
    {
        canDamage = false;
        if (col != null)
            col.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if ((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }
}
