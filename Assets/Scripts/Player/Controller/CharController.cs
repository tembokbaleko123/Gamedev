using UnityEngine;
using UnityEngine.InputSystem;

public class CharController : MonoBehaviour, InputChar.ICharacterActions
{
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    InputChar input;

    Vector2 currentMovement;
    bool movementPressed;
    bool runToggled;

    // -----------------------------------------------------------------------

    void Awake()
    {
        input = new InputChar();
    }

    void Start()
    {
        animator      = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        // 🔥 FIX: ambil movement langsung (bukan dari event)
        currentMovement = input.Character.Movement.ReadValue<Vector2>();
        movementPressed = currentMovement.sqrMagnitude > 0.01f;

        if (!movementPressed)
            runToggled = false;

        HandleMovement();
        HandleRotation();
    }

    // -----------------------------------------------------------------------
    // InputChar.IControllerActions

    public void OnMovement(InputAction.CallbackContext context)
    {
        currentMovement = context.ReadValue<Vector2>();
        movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // Hanya jalankan saat tombol baru ditekan (performed)
        // Abaikan started dan canceled agar toggle tidak balik sendiri
        if (!context.performed) return;

        if (movementPressed)
            runToggled = !runToggled;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    // -----------------------------------------------------------------------

    void HandleRotation()
    {
        if (!movementPressed) return;

        Vector3 positionToLookAt = transform.position + new Vector3(currentMovement.x, 0, currentMovement.y);
        transform.LookAt(positionToLookAt);
    }

    void HandleMovement()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        if (!movementPressed)
        {
            if (isWalking) animator.SetBool(isWalkingHash, false);
            if (isRunning) animator.SetBool(isRunningHash, false);
            return;
        }

        if (runToggled)
        {
            if (isWalking)  animator.SetBool(isWalkingHash, false);
            if (!isRunning) animator.SetBool(isRunningHash, true);
        }
        else
        {
            if (isRunning)  animator.SetBool(isRunningHash, false);
            if (!isWalking) animator.SetBool(isWalkingHash, true);
        }
    }

    // -----------------------------------------------------------------------

    void OnEnable()
    {
        input.Character.AddCallbacks(this);
        input.Character.Enable();
    }

    void OnDisable()
    {
        input.Character.RemoveCallbacks(this);
        input.Character.Disable();
    }

    void OnDestroy()
    {
        input.Dispose();
    }
}