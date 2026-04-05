# Death SFX

Suara saat character (enemy atau player) mati.

## File yang Direkomendasikan

| Nama File | Keterangan | Durasi |
|-----------|------------|--------|
| `Death_Grunt_Male_01.wav` | Suara grunt saat mati | 0.5-1.5s |
| `Death_Grunt_Male_02.wav` | Variasi grunt | 0.5-1.5s |
| `Death_Scream_Short_01.wav` | Teriakan pendek | 0.5-1s |
| `Death_Gasp_01.wav` | Suara napas terakhir | 0.5-1s |

## Keyword Pencarian

- "death grunt"
- "male death"
- "dying breath"
- "death scream"
- "character death"

## Cara Pakai

Dipanggil saat `Die()` di Health.cs:

```csharp
public AudioClip deathSFX;

void Die() {
    if (deathSFX != null && audioSource != null) {
        audioSource.PlayOneShot(deathSFX);
    }
    // ...
}
```

## Catatan

- Death SFX lebih panjang dari hit/swing
- Bisa kombinasi: death sound + fall sound
