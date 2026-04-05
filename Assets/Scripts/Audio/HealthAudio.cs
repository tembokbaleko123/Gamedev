using UnityEngine;

     [RequireComponent(typeof(AudioSource))]
     public class HealthAudio : MonoBehaviour
     {
         [Header("Hit SFX")]
         [Tooltip("Suara saat kena hit. Random dipilih jika ada multiple.")]
         public AudioClip[] hitSFX;

         [Header("Death SFX")]
         [Tooltip("Suara saat mati")]
         public AudioClip deathSFX;

         [Header("Settings")]
         [Range(0.8f, 1.2f)]
         public float pitchVariation = 0.1f; // Variasi pitch untuk variasi
         [Range(0.5f, 1.5f)]
         public float hitVolume = 1f;
         [Range(0.5f, 1.5f)]
         public float deathVolume = 1f;

         private AudioSource audioSource;
         private Health health;

         void Awake()
         {
             audioSource = GetComponent<AudioSource>();
             health = GetComponent<Health>();
         }

         void OnEnable()
        {
         Debug.Log("HealthAudio OnEnable - Subscribing to events");

         if (health != null)
         {
             Debug.Log("Health component found, subscribing...");
             health.OnDamageTaken += PlayHitSound;
             health.OnDeath += PlayDeathSound;
         }
         else
         {
             Debug.LogError("Health component NOT found!");
         }
     }

         void OnDisable()
         {
             if (health != null)
             {
                 health.OnDamageTaken -= PlayHitSound;
                 health.OnDeath -= PlayDeathSound;
             }
         }

         public void PlayHitSound(int damage)
         {

            Debug.Log("PlayHitSound called! Damage: " + damage);

             if (hitSFX == null || hitSFX.Length == 0) 
             {
                 Debug.LogWarning("hitSFX is empty or null!");
                 return;
             }

             // Pilih random clip
             int index = Random.Range(0, hitSFX.Length);
             AudioClip clip = hitSFX[index];

             // Variasi pitch
             float pitch = 1f + Random.Range(-pitchVariation, pitchVariation);

             PlaySound(clip, hitVolume, pitch);
         }

         public void PlayDeathSound()
         {
             if (deathSFX == null) return;

             PlaySound(deathSFX, deathVolume, 1f);
         }

         private void PlaySound(AudioClip clip, float volume, float pitch)
         {
             if (clip == null || audioSource == null) return;

             // Simpan pitch asli
             float originalPitch = audioSource.pitch;

             audioSource.pitch = pitch;
             audioSource.PlayOneShot(clip, volume);

             // Reset pitch (untuk SFX berikutnya)
             audioSource.pitch = originalPitch;
         }
     }