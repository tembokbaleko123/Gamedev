using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Kamera yang mengikuti karakter dengan kontrol rotasi mouse (Input System).
///
/// Setup:
/// 1. KELUARKAN Main Camera dari hierarchy Y Bot (drag ke root Scene).
/// 2. Attach script ini ke Main Camera.
/// 3. Assign Target = Y Bot di Inspector.
/// 4. Pastikan Input System package aktif dan ada action "Look" untuk mouse delta.
///
/// Hierarchy yang benar:
///   Scene
///   ├── Y Bot          ← CharController.cs
///   └── Main Camera    ← FollowCamera.cs (BUKAN child Y Bot)
/// </summary>
public class FollowCamera : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Assign Y Bot GameObject di sini.")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float distance = 5f;      // Jarak dari karakter
    [SerializeField] private float height = 2f;        // Tinggi kamera relatif target
    [SerializeField] private float smoothSpeed = 10f;  // Kelembutan follow

    [Header("Mouse Rotation")]
    [SerializeField] private float mouseSensitivityX = 0.5f;   // Sensitivitas rotasi horizontal
    [SerializeField] private float mouseSensitivityY = 0.3f;   // Sensitivitas rotasi vertikal
    [SerializeField] private float minVerticalAngle = -30f;    // Batas bawah pandangan
    [SerializeField] private float maxVerticalAngle = 60f;     // Batas atas pandangan

    [Header("Look At Offset")]
    [Tooltip("Titik yang dilihat kamera (offset dari posisi karakter).")]
    [SerializeField] private Vector3 lookAtOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Input")]
    [Tooltip("Reference ke Input Action Asset (opsional - bisa pakai default).")]
    [SerializeField] private InputActionAsset inputActions;
    [Tooltip("Nama action map yang berisi Look action.")]
    [SerializeField] private string actionMapName = "Player";
    [Tooltip("Nama action untuk look/mouse delta.")]
    [SerializeField] private string lookActionName = "Look";

    private float currentYaw = 0f;    // Rotasi horizontal (kiri/kanan)
    private float currentPitch = 20f; // Rotasi vertikal (atas/bawah)
    private Vector3 currentPosition;
    private Quaternion currentRotation;
    private InputAction lookAction;
    private Vector2 lookInput;

    // -------------------------------------------------------------------------
    private void Awake()
    {
        SetupInput();
    }

    private void SetupInput()
    {
        // Coba dapatkan dari serialized field dulu
        if (inputActions != null)
        {
            var actionMap = inputActions.FindActionMap(actionMapName);
            if (actionMap != null)
            {
                lookAction = actionMap.FindAction(lookActionName);
            }
        }

        // Kalau belum ketemu, coba cari di PlayerInput component
        if (lookAction == null)
        {
            var playerInput = FindObjectOfType<PlayerInput>();
            if (playerInput != null)
            {
                lookAction = playerInput.actions.FindAction(lookActionName);
            }
        }

        // Kalau masih belum ketemu, buat input action default
        if (lookAction == null)
        {
            lookAction = new InputAction("Look", binding: "<Mouse>/delta");
            lookAction.Enable();
        }

        // Subscribe ke performed callback
        if (lookAction != null)
        {
            lookAction.performed += OnLookPerformed;
            lookAction.canceled += OnLookCanceled;
            lookAction.Enable();
        }
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    private void Start()
    {
        if (target != null)
        {
            // Inisialisasi posisi kamera
            currentPosition = transform.position;
            currentRotation = transform.rotation;

            // Hitung yaw awal dari arah kamera ke target
            Vector3 dirToTarget = target.position - transform.position;
            currentYaw = Mathf.Atan2(dirToTarget.x, dirToTarget.z) * Mathf.Rad2Deg;
        }

        // Lock cursor untuk rotasi yang lebih baik
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        // Unsubscribe dan unlock cursor
        if (lookAction != null)
        {
            lookAction.performed -= OnLookPerformed;
            lookAction.canceled -= OnLookCanceled;
            lookAction.Disable();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        HandleMouseInput();
        UpdateCameraPosition();
    }

    // -------------------------------------------------------------------------
    private void HandleMouseInput()
    {
        // Baca input dari lookInput (sudah diupdate via callback)
        float mouseX = lookInput.x * mouseSensitivityX;
        float mouseY = lookInput.y * mouseSensitivityY;

        // Update rotasi
        currentYaw += mouseX;
        currentPitch -= mouseY; // Invert Y untuk kontrol natural

        // Clamp rotasi vertikal
        currentPitch = Mathf.Clamp(currentPitch, minVerticalAngle, maxVerticalAngle);
    }

    private void UpdateCameraPosition()
    {
        // Hitung rotasi dari yaw dan pitch
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // Hitung posisi kamera berdasarkan rotasi dan offset
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);
        Vector3 targetPosition = target.position + lookAtOffset;
        Vector3 desiredPosition = targetPosition + offset + Vector3.up * height;

        // Smooth follow tanpa goyangan
        currentPosition = Vector3.Lerp(currentPosition, desiredPosition, smoothSpeed * Time.deltaTime);

        // Update posisi kamera
        transform.position = currentPosition;

        // Look at target dengan stabil
        Vector3 lookTarget = targetPosition;
        Vector3 direction = lookTarget - transform.position;
        if (direction != Vector3.zero)
        {
            currentRotation = Quaternion.LookRotation(direction);
            transform.rotation = currentRotation;
        }
    }

    // -------------------------------------------------------------------------
    #region Public API

    /// <summary>Set target karakter secara runtime.</summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            currentPosition = transform.position;
        }
    }

    /// <summary>Reset rotasi kamera ke belakang target.</summary>
    public void ResetRotation()
    {
        if (target != null)
        {
            currentYaw = target.eulerAngles.y;
            currentPitch = 20f;
        }
    }

    /// <summary>Lock/unlock cursor.</summary>
    public void SetCursorLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    #endregion
}