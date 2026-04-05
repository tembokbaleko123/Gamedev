using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField] private TrailRenderer trail;
    
    [Header("Particle Slash")]
    [SerializeField] private ParticleSystem slashParticle;
    [SerializeField] private Transform slashSpawnPoint;

    void Awake()
    {
        if (trail == null)
            trail = GetComponent<TrailRenderer>();
        
        if (trail != null)
            trail.enabled = false;
    }

    public void StartTrail()
    {
        if (trail != null)
        {
            trail.enabled = true;
            trail.Clear();
        }
    }

    public void StopTrail()
    {
        if (trail != null)
            trail.enabled = false;
    }

    public void SpawnSlashParticle()
     {
         if (slashParticle != null && slashSpawnPoint != null)
         {
            ParticleSystem instance = Instantiate(slashParticle, slashSpawnPoint.position,
            slashSpawnPoint.rotation);

            // Hancurkan setelah selesai (jangan langsung)
            float duration = instance.main.duration + instance.main.startLifetime.constant;
            Destroy(instance.gameObject, duration);
         }
     }
}
