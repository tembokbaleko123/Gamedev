using UnityEngine;

public class EnemyAnimationEventRelay : MonoBehaviour
{
    [Header("References")]
    public EnemyAttackCollider hitCollider;

    public void StartDamage()
    {
        if (hitCollider != null)
            hitCollider.StartDamage();
    }

    public void EndDamage()
    {
        if (hitCollider != null)
            hitCollider.EndDamage();
    }
}
