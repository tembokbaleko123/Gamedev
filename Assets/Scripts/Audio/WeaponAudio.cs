using UnityEngine;

     [RequireComponent(typeof(AudioSource))]
     public class WeaponAudio : MonoBehaviour
     {
         [Header("Swing SFX")]
         [Tooltip("Suara saat ayunan pedang. Random dipilih.")]
         public AudioClip[] swingSFX;

         [Header("Settings")]
         [Range(0.8f, 1.2f)]
         public float pitchVariation = 0.1f;
         [Range(0.5f, 1.5f)]
         public float volume = 1f;

         private AudioSource audioSource;

         void Awake()
         {
             audioSource = GetComponent<AudioSource>();
         }

         // Dipanggil dari Animation Event di awal animasi attack
         public void PlaySwingSound()
         {
             if (swingSFX == null || swingSFX.Length == 0) return;

             int index = Random.Range(0, swingSFX.Length);
             AudioClip clip = swingSFX[index];

             float pitch = 1f;

             if (audioSource != null && clip != null)
             {
                 audioSource.pitch = pitch;
                 audioSource.PlayOneShot(clip, volume);
             }
         }
     }