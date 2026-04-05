# Hit SFX

Suara impact saat serangan mengenai target.

## File yang Direkomendasikan

| Nama File | Keterangan | Durasi |
|-----------|------------|--------|
| `Sword_Hit_Body_01.wav` | Pedang mengenai tubuh/flesh | 0.2-0.5s |
| `Sword_Hit_Body_02.wav` | Variasi hit body | 0.2-0.5s |
| `Sword_Hit_Metal_01.wav` | Pedang mengenai armor/logam | 0.2-0.5s |
| `Sword_Hit_Wood_01.wav` | Pedang mengenai shield kayu | 0.2-0.5s |
| `Punch_Hit_01.wav` | Tinju/tangan kosong | 0.1-0.3s |

## Keyword Pencarian (freesound.org)

- "sword hit flesh"
- "body impact"
- "slash impact"
- "metal hit"
- "knife stab"

## Cara Setup di Unity

1. Import file .wav ke folder ini
2. Select file → Inspector → Import Settings:
   - Load In Background: false
   - Preload Audio Data: true
3. Drag file ke field `hitSFX` di script Health.cs

## Script Reference

```csharp
// Di Health.cs
public AudioClip hitSFX;
private AudioSource audioSource;

void Start() {
    audioSource = GetComponent<AudioSource>();
}

public void TakeDamage(int damage) {
    // ... kode damage ...
    
    if (hitSFX != null && audioSource != null) {
        audioSource.PlayOneShot(hitSFX);
    }
}
```

## Variasi

Untuk game yang lebih baik, buat variasi hit sound:
- Minimal 2-3 variasi per tipe hit
- Random pilih saat runtime supaya tidak monoton
