     using UnityEngine;

     public class AudioEventRelay : MonoBehaviour
     {
         public WeaponAudio weaponAudio;
         private bool isSwinging = false;

         public void PlaySwingSound()
        {
            Debug.Log("PlaySwingSound called");

            if (isSwinging)
            {
                Debug.Log("Blocked - still swinging");
                return;
            }

            if (weaponAudio == null)
            {
                Debug.LogError("WeaponAudio is NULL! Assign di Inspector.");
                return;
            }

            isSwinging = true;
            weaponAudio.PlaySwingSound();
        }


         // Dipanggil dari PlayerAttack.EndAttack()
         public void ResetSwing()
         {
             isSwinging = false;
             Debug.Log("Swing reset - ready for next attack");
         }
     }
