using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int damage = 10;
    public LayerMask enemyLayer;

    private bool canDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Health health = other.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    // Dipanggil dari animasi
    public void StartDamage()
    {
        canDamage = true;
    }

    public void EndDamage()
    {
        canDamage = false;
    }
}