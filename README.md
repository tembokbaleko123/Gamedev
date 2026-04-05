# Hack and Slash Unity Game

Third-person hack and slash game built with Unity 3D (URP) and Unity Input System.

## 7-Day Development Plan

| Day | Topic | Status |
|-----|-------|--------|
| Day 1 | Combat Dasar + Health System | ✅ DONE |
| Day 2 | Hit Feedback (VFX, SFX, Squash) | ✅ DONE |
| Day 3 | Weapon Feel (Trail, Slash, Swing) | ✅ DONE |
| Day 4 | Enemy AI (Chase, Rotate, NavMeshAgent) | ✅ DONE |
| Day 5 | Enemy Attack + Player Damage | ✅ DONE |
| Day 6 | Combo + Arena + Win/Lose | ⏳ IN PROGRESS |
| Day 7 | Polish + Win/Lose | ⏳ TODO |

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

---

## Current Progress

### Day 5 COMPLETED ✅
- Enemy AI with attack animations
- Combo system (Attack1 → Attack2 → Attack3)
- EnemyAttackCollider for hit detection
- PlayerHealth.cs with damage handling
- PlayerHealthUI.cs with slider

### Day 6 TODO
- Arena/Wave system
- Win condition (kill all enemies)
- Lose condition (player HP = 0)

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── Controller/
│   │   │   ├── CharController.cs    # Movement, Sprint, Rotation
│   │   │   └── FollowCamera.cs     # Third-person camera
│   │   ├── Attack/
│   │   │   ├── PlayerAttack.cs     # Attack trigger
│   │   │   ├── WeaponDamage.cs     # Damage collider
│   │   │   └── AnimationEventRelay.cs
│   │   ├── PlayerHealth.cs         # HP system
│   │   └── PlayerHealthUI.cs       # Health slider
│   ├── Enemy/
│   │   ├── EnemyAi.cs              # AI state machine
│   │   ├── Health.cs               # Enemy HP
│   │   ├── EnemyAttackCollider.cs   # Hit detection
│   │   └── EnemyAnimationEventRelay.cs
│   ├── Combat/
│   │   └── WeaponTrail.cs         # Slash trail
│   └── Audio/
│       └── ...                     # SFX handlers
├── Characters/
│   └── X Bot/
│       └── Animations/
│           ├── X Bot Controller.controller
│           ├── Attack/
│           │   ├── Attack1.anim    # Boxing-Blow Cut
│           │   ├── Attack2.anim    # Boxing-Left Hand
│           │   └── Attack3.anim    # Punching
│           ├── Locomotion/
│           │   └── Standard Run.anim
│           └── Dying.anim
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

## Credits

- Character: X Bot
- Audio: Various sources
- Particle Effects: Unity Particle Pack
