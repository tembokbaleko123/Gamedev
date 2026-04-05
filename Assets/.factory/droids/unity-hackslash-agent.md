# Unity Hack and Slash AI Agent Context

## Project Overview
- **Engine**: Unity 3D with Universal Render Pipeline (URP)
- **Input System**: Unity Input System (New Input System)
- **Genre**: Third-Person Hack and Slash
- **Current Progress**: Hari 3 - Weapon Feel (DONE), Next: Hari 4 - Enemy AI

## Folder Structure
```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── Controller/
│   │   │   ├── CharController.cs      # Movement, Sprint, Rotation
│   │   │   └── FollowCamera.cs        # Third-person camera orbit
│   │   └── Attack/
│   │       ├── PlayerAttack.cs        # Attack trigger, combo state
│   │       ├── WeaponDamage.cs        # Damage collider, LayerMask enemy
│   │       └── AnimationEventRelay.cs # Bridge pattern for animation events
│   ├── Enemy/
│   │   ├── Enemy.cs                   # Currently empty - NEEDS AI
│   │   ├── Health.cs                  # HP system, death, hit feedback
│   │   └── EnemyHealthUI.cs           # Health bar slider
│   ├── Combat/
│   │   └── WeaponTrail.cs             # Trail renderer, slash particles
│   └── Audio/
│       ├── AudioEventRelay.cs         # Swing SFX relay
│       ├── WeaponAudio.cs             # Random pitch swing sounds
│       ├── FootstepAudio.cs           # Movement SFX
│       └── HealthAudio.cs             # Damage/death SFX
├── InputManager/
│   └── InputChar.cs                   # Auto-generated Input Actions
```

## Code Patterns Used

### 1. Input System Pattern
```csharp
using UnityEngine.InputSystem;

public class Example : MonoBehaviour, InputChar.ICharacterActions
{
    InputChar input;
    
    void Awake() { input = new InputChar(); }
    void OnEnable() { input.Character.AddCallbacks(this); input.Character.Enable(); }
    void OnDisable() { input.Character.RemoveCallbacks(this); input.Character.Disable(); }
    
    public void OnMovement(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }
    public void OnAttack(InputAction.CallbackContext context) { }
}
```

### 2. Animation Event Relay Pattern
Used when Animator is on parent but logic is in child component.
```csharp
public class AnimationEventRelay : MonoBehaviour
{
    public WeaponDamage weapon;
    
    // Called from Animation Event
    public void StartDamage() { weapon.StartDamage(); }
    public void EndDamage() { weapon.EndDamage(); }
}
```

### 3. Health System Pattern
```csharp
public class Health : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    public event Action<int> OnDamageTaken;
    public event Action OnDeath;
    
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        OnDamageTaken?.Invoke(damage);
        
        // Visual feedback
        if (hitEffectPrefab != null) Instantiate(hitEffectPrefab, ...);
        transform.localScale = new Vector3(1.2f, 0.8f, 1.2f); // Squash
        Invoke(nameof(ResetScale), 0.1f);
        
        if (currentHealth <= 0) Die();
    }
    
    void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        animator.SetBool("IsDying", true);
        Destroy(gameObject, 3f);
    }
}
```

### 4. Weapon Damage Pattern
```csharp
public class WeaponDamage : MonoBehaviour
{
    public int damage = 10;
    public LayerMask enemyLayer;
    private bool canDamage = false;
    
    // Called from animation
    public void StartDamage() { canDamage = true; }
    public void EndDamage() { canDamage = false; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Health health = other.GetComponent<Health>();
            if (health != null) health.TakeDamage(damage);
        }
    }
}
```

### 5. Audio Pattern
```csharp
[RequireComponent(typeof(AudioSource))]
public class WeaponAudio : MonoBehaviour
{
    public AudioClip[] swingSFX;
    [Range(0.8f, 1.2f)] public float pitchVariation = 0.1f;
    private AudioSource audioSource;
    
    void Awake() { audioSource = GetComponent<AudioSource>(); }
    
    public void PlaySwingSound()
    {
        if (swingSFX == null || swingSFX.Length == 0) return;
        int index = Random.Range(0, swingSFX.Length);
        audioSource.PlayOneShot(swingSFX[index], volume);
    }
}
```

## Input Mappings
| Action | Binding |
|--------|---------|
| Movement | WASD (2D Vector) |
| Sprint | Left Shift (toggle) |
| Attack | Mouse Left Button |
| Camera | Mouse Delta |

## Animation Parameters
| Parameter | Type | Usage |
|-----------|------|-------|
| isWalking | bool | Player walking state |
| isRunning | bool | Player sprint state |
| Slash | trigger | Attack trigger |
| IsDying | bool | Enemy death state |

## 7-Day Workflow Progress
- ✅ Hari 1: Combat Dasar + Health System
- ✅ Hari 2: Hit Feedback (VFX, SFX, Squash)
- ✅ Hari 3: Weapon Feel (Trail, Slash, Swing)
- ✅ Hari 4: Enemy AI (Chase, Rotate, Stop Distance, Dying Animation Fix)
- ⏳ Hari 4 POLISH (Optional)
- ⏳ Hari 5: Enemy Attack + Player Damage
- ⏳ Hari 6: Combo + Arena
- ⏳ Hari 7: Polish + Win/Lose

## Hari 4 COMPLETED ✅
### Features Implemented:
- ✅ EnemyAI.cs dengan NavMeshAgent
- ✅ Chase player dalam range
- ✅ Stop dan face player saat dekat
- ✅ Smooth rotation
- ✅ IsRunning animation parameter
- ✅ Health.cs dengan IsDying flag
- ✅ Dying animation (no loop, Can Transition to Self = false)
- ✅ Auto-disable NavMeshAgent saat mati
- ✅ Destroy object after 3s

### Files Created/Modified:
- `Scripts/Enemy/EnemyAI.cs`
- `Scripts/Enemy/Health.cs` (updated)

## Naming Conventions
- Private: camelCase (`currentHealth`, `canDamage`)
- Public: PascalCase (`MaxHealth`, `EnemyLayer`)
- Events: OnXxx (`OnDamageTaken`, `OnDeath`)
- Animator Hashes: xxxHash (`isWalkingHash`, `isRunningHash`)
- SerializeField for Inspector exposure

## Dependencies
- Unity Input System package
- Animator component on character root
- AudioSource component for SFX
- TrailRenderer for weapon trail
- ParticleSystem for hit/slash effects

---

# WORKFLOW MODE: COPY-PASTE + MANUAL GUIDANCE

## Cara Kerja
**Default Mode**: Saya akan memberikan kode lengkap + instruksi step-by-step. Kamu copy-paste dan buat file manual di Unity.

**Auto-Create Mode**: Saya akan membuat file otomatis jika kamu bilang:
- "buatkan filenya"
- "create file"
- "buat otomatis"

⚠️ **Rules Auto-Create**:
- Hanya untuk file di folder `Scripts/` (C# scripts)
- File lain (.unity, .prefab, asset) tetap manual/copy-paste
- Akan saya konfirmasi dulu sebelum create

### Format Instruksi:
```
📁 FILE: [Path lengkap]
📝 AKSI: [Create/Edit/Replace]
📋 STEP:
1. [Langkah setup di Unity]
2. [Langkah berikutnya]

```csharp
// [Kode lengkap siap copy-paste]
```

⚠️ CATATAN: [Tips penting]
```

## Contoh Workflow (Hari 4 - Enemy AI)

### Step 1: Create EnemyAI.cs
```
📁 FILE: Assets/Scripts/Enemy/EnemyAI.cs
📝 AKSI: Create
📋 STEP:
1. Buka Unity Project window
2. Navigate ke Assets/Scripts/Enemy/
3. Right-click > Create > C# Script
4. Name: "EnemyAI"
5. Double-click untuk edit di VS/VS Code

```csharp
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    
    [Header("Movement")]
    public float chaseRange = 10f;
    public float stopDistance = 2f;
    public float rotationSpeed = 5f;
    
    private NavMeshAgent agent;
    private Animator animator;
    
    // Animator hashes
    private int isWalkingHash;
    private int isRunningHash;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }
    
    void Start()
    {
        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= chaseRange && distanceToPlayer > stopDistance)
        {
            ChasePlayer();
        }
        else
        {
            StopChasing();
        }
        
        UpdateAnimations();
    }
    
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        
        // Rotate to face player while moving
        RotateTowards(player.position);
    }
    
    void StopChasing()
    {
        agent.isStopped = true;
    }
    
    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0; // Keep rotation on Y axis only
        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
    
    void UpdateAnimations()
    {
        bool isMoving = agent.velocity.sqrMagnitude > 0.1f && !agent.isStopped;
        animator.SetBool(isWalkingHash, isMoving);
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualize chase range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        
        // Visualize stop distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
```

⚠️ CATATAN:
- Pastikan enemy punya NavMeshAgent component
- Bake NavMesh di Scene: Window > AI > Navigation > Bake
- Player tag harus di-set ke "Player"
- Animator needs isWalking parameter (bool)
```

### Step 2: Setup NavMesh
```
📁 FILE: Scene (SampleScene)
📝 AKSI: Edit Scene
📋 STEP:
1. Pilih floor/ground object di hierarchy
2. Window > AI > Navigation
3. Tab "Object" > Check "Navigation Static"
4. Tab "Bake" > Click "Bake"
5. Tunggu sampai NavMesh muncul (biru di scene)

⚠️ CATATAN:
- Pastikan ground mesh collider aktif
- NavMesh akan muncul sebagai overlay biru
- Enemy hanya bisa jalan di area biru
```

### Step 3: Setup Enemy Prefab
```
📁 FILE: Enemy prefab di Hierarchy
📝 AKSI: Edit Inspector
📋 STEP:
1. Select enemy GameObject
2. Add Component: Nav Mesh Agent
3. Set Speed: 3.5
4. Set Angular Speed: 120
5. Set Acceleration: 8
6. Set Stopping Distance: 2
7. Assign EnemyAI.cs script
8. Assign Player ke slot "Player" (drag dari hierarchy)
9. Pastikan Animator ada dengan parameter "isWalking"

⚠️ CATATAN:
- Kalau Player null, script akan auto-find via tag "Player"
- Stopping Distance di NavMeshAgent harus match dengan stopDistance di script
```

## Format untuk Hari Berikutnya

Setiap hari akan mengikuti format ini:
1. **Overview** — Apa yang akan dibuat
2. **File-by-file** — Setiap file dengan kode lengkap
3. **Unity Setup** — Langkah di Inspector/Scene
4. **Testing** — Cara test hasilnya
5. **Debug Tips** — Kalau ada masalah

---

# HARI 4 POLISH OPTIONS (Optional)

Sebelum lanjut ke Hari 5, bisa polish Hari 4 dulu:

## Polish Options:

### Option A: Smooth Chase (⭐ Low Effort, ⭐⭐ Medium Impact)
```csharp
// Enemy slowdown saat dekat stopDistance
public float smoothStopDistance = 0.5f;
float distance = Vector3.Distance(transform.position, player.position);
if (distance < stopDistance + smoothStopDistance)
{
    float speedRatio = (distance - stopDistance) / smoothStopDistance;
    agent.speed = Mathf.Lerp(0, 3.5f, speedRatio);
}
```

### Option B: Random Path Offset (⭐ Low Effort, ⭐⭐⭐ High Impact)
```csharp
// Enemy tidak jalan lurus, ada randomization
public float pathfindingOffset = 0.5f;
Vector3 offset = Random.insideUnitSphere * pathfindingOffset;
agent.SetDestination(player.position + offset);
```

### Option C: Visual Chase Feedback (⭐⭐ Medium Effort, ⭐⭐⭐ High Impact)
```csharp
// Material berubah warna saat chase
// Atau instantiate exclamation mark icon di atas enemy
```

### Option D: Enemy Footstep Audio (⭐⭐ Medium Effort, ⭐⭐ Medium Impact)
```csharp
// Copy pattern FootstepAudio.cs untuk enemy
// Trigger dari animation event
```

### Option E: Health Bar Face Camera (⭐ Low Effort, ⭐⭐ Medium Impact)
```csharp
// EnemyHealthUI.cs update:
void LateUpdate() { transform.rotation = Camera.main.transform.rotation; }
```

### Option F: Better Animation Blend (⭐ Low Effort, ⭐⭐ Medium Impact)
```csharp
// Animator: Transition Duration 0.15 (bukan 0)
// Has Exit Time: false
```

## Rekomendasi Polish (Pilih 2-3):
| Prioritas | Polish | Effort | Impact |
|-----------|--------|--------|--------|
| 1 | Smooth Chase | ⭐ Low | ⭐⭐ Medium |
| 2 | Health Bar Face Camera | ⭐ Low | ⭐⭐ Medium |
| 3 | Random Path Offset | ⭐ Low | ⭐⭐⭐ High |

---

# READY: HARI 5 - ENEMY ATTACK & PLAYER DAMAGE

## Target Hari 5:
- Enemy bisa menyerang player
- Player punya HP
- Player bisa mati
- Lose condition

## Files yang akan dibuat:
1. `PlayerHealth.cs` (baru)
2. `EnemyAttack.cs` (baru)
3. Update `EnemyAI.cs` (tambah attack state)

---

## PILIH SEKARANG:

**A.** LANJUT HARI 5 (Skip Polish)
**B.** POLISH DULU (Pilih 2-3 opsi di atas)
**C.** SEMUA POLISH (Lanjut Hari 5 nanti)
