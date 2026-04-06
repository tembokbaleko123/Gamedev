# Hack and Slash Unity Game

Third-person hack and slash game built with Unity 3D (URP) and Unity Input System.

## 7-Day Development Plan

| Day | Topic | Status |
|-----|-------|--------|
| Day 1 | Combat Dasar + Health System | DONE |
| Day 2 | Hit Feedback (VFX, SFX, Squash) | DONE |
| Day 3 | Weapon Feel (Trail, Slash, Swing) | DONE |
| Day 4 | Enemy AI (Chase, Rotate, NavMeshAgent) | DONE |
| Day 5 | Enemy Attack + Player Damage | DONE |
| Day 5.5 | Player Combo Attack (3-hit) | DONE |
| Day 6 | Arena + Wave System + Win/Lose | DONE |
| Day 7 | Polish | DONE |

---

## Completed Features

### Day 1 - Combat & Health
- Player attack system
- Health component for damage handling

### Day 2 - Hit Feedback
- Visual effects (VFX)
- Sound effects (SFX)
- Squash and stretch animation on hit

### Day 3 - Weapon Feel
- Weapon trail renderer
- Slash particle effects
- Swing sound effects

### Day 4 - Enemy AI
- NavMeshAgent navigation
- Chase player within range
- Smooth rotation towards player
- Stop and face player at distance
- Enemy death animation

### Day 5 - Enemy Attack + Player Damage
- Enemy attack animations (Attack1, Attack2, Attack3)
- Combo system with attack transitions
- EnemyAttackCollider for hit detection
- PlayerHealth system with damage handling
- Player health UI slider
- Player death and scene restart

### Day 5.5 - Player Combo Attack
- 3-hit combo system with input buffering
- Animation events for damage sync
- Combo counter (1 -> 2 -> 3 -> 1 loop)

### Day 6 - Arena + Wave System
- WaveData ScriptableObject for wave configuration
- ArenaManager with enemy spawning
- GameManager with game states (PreGame, Playing, Won, Lost)
- GameUI with Start/Gameplay/GameOver/Win screens
- Player spawn point reset on restart

### Day 7 - Polish
- Wave UI timing fix
- Enemy count tracking
- Player dying animation fix

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── Controller/
│   │   │   ├── CharController.cs    # Movement, Sprint, Rotation
│   │   │   └── FollowCamera.cs      # Third-person camera
│   │   ├── Attack/
│   │   │   ├── PlayerAttack.cs       # 3-hit combo system
│   │   │   ├── WeaponDamage.cs       # Damage collider
│   │   │   └── AnimationEventRelay.cs
│   │   ├── PlayerHealth.cs           # HP system
│   │   └── PlayerHealthUI.cs         # Health slider
│   ├── Enemy/
│   │   ├── EnemyAi.cs               # AI state machine
│   │   ├── Health.cs                # Enemy HP
│   │   ├── EnemyAttackCollider.cs    # Hit detection
│   │   └── EnemyAnimationEventRelay.cs
│   ├── Combat/
│   │   └── WeaponTrail.cs           # Slash trail
│   ├── Arena/
│   │   ├── ArenaManager.cs          # Wave spawning
│   │   ├── GameManager.cs           # Game state
│   │   ├── GameUI.cs                # UI screens
│   │   └── WaveData.cs              # Wave config
│   └── Audio/
│       ├── AudioEventRelay.cs       # Swing SFX
│       ├── WeaponAudio.cs           # Random pitch
│       ├── FootstepAudio.cs         # Placeholder
│       └── HealthAudio.cs           # Hit/death SFX
├── Characters/
│   ├── X Bot/
│   └── Y Bot/
│       └── Animations/
│           └── Y Bot Anim.controller
├── InputManager/
│   └── InputChar.cs                 # Input actions
└── Scenes/
    └── SampleScene.unity
```

---

## Controls

| Action | Input |
|--------|-------|
| Movement | WASD |
| Sprint | Left Shift (toggle) |
| Attack | Mouse Left Button |
| Camera | Mouse Delta |

---

## Setup

1. Open project in Unity 2023+
2. Open `Scenes/SampleScene.unity`
3. Play!

---

## TODO (Future)

- FootstepAudio implementation
- Screen shake on damage
- More polish...

---

## Credits

### Free Assets Used

- **Characters (Y Bot / X Bot)**
  - Source: Mixamo (https://www.mixamo.com)
  - Free 3D characters with animations

- **Audio / Sound Effects**
  - Source: Freesound (https://freesound.org)
  - Source: Various free sound libraries

- **Environment**
  - Unity Terrain URP Demo Scene: https://assetstore.unity.com/packages/3d/environments/unity-terrain-urp-demo-scene-213197

- **Particle Effects**
  - Unity Particle Pack (built-in)

---

## License

This project uses free assets for development purposes. Please refer to each asset provider's license for usage terms.
