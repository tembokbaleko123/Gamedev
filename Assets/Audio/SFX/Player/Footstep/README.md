# Footstep SFX

Suara langkah kaki player.

## File yang Direkomendasikan

| Nama File | Keterangan | Durasi |
|-----------|------------|--------|
| `Footstep_Grass_01.wav` | Jalan di rumput | 0.1-0.3s |
| `Footstep_Stone_01.wav` | Jalan di batu | 0.1-0.3s |
| `Footstep_Dirt_01.wav` | Jalan di tanah | 0.1-0.3s |
| `Footstep_Run_01.wav` | Lari (variasi) | 0.1-0.2s |

## Cara Pakai

Dipanggil via Animation Event di animasi walk/run:
- Event di frame saat kaki menyentuh tanah
- Biasanya 2 event per siklus jalan (kiri dan kanan)

## Script Example

```csharp
public AudioClip[] footstepSounds;

public void PlayFootstep() {
    if (footstepSounds.Length > 0) {
        int index = Random.Range(0, footstepSounds.Length);
        audioSource.PlayOneShot(footstepSounds[index]);
    }
}
```

## Tips

- Buat banyak variasi (minimal 4-6) supaya tidak repetitive
- Random pitch sedikit (0.95 - 1.05) untuk variasi tambahan
