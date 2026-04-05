     using UnityEngine;
     using UnityEngine.InputSystem;

     public class PlayerAttack : MonoBehaviour
     {
         private Animator animator;
         private InputChar inputActions;
         private bool isAttacking = false;

         void Awake()
         {
             inputActions = new InputChar();
         }

         void OnEnable()
         {
             inputActions.Enable();
             inputActions.Character.Attack.performed += OnAttack;
         }

         void OnDisable()
         {
             inputActions.Character.Attack.performed -= OnAttack;
             inputActions.Disable();
         }

         void Start()
         {
             animator = GetComponent<Animator>();
         }

         private void OnAttack(InputAction.CallbackContext context)
         {
             if (!isAttacking)
             {
                 isAttacking = true;
                 animator.SetTrigger("Slash");
             }
         }

         // DIPANGGIL DI AKHIR ANIMASI
         public void EndAttack()
         {
             isAttacking = false;

             // Panggil ResetSwing di AudioEventRelay
             var relay = GetComponent<AudioEventRelay>();
             if (relay != null)
             {
                 relay.ResetSwing();
             }
         }
     }