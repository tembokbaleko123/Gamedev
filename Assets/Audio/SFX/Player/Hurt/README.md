# Hurt SFX

Suara saat player kena damage dari enemy.

## File yang Direkomendasikan

| Nama File | Keterangan | Durasi |
|-----------|------------|--------|
| `Hurt_Grunt_01.wav` | Grunt saat kena hit | 0.3-0.6s |
| `Hurt_Grunt_02.wav` | Variasi grunt | 0.3-0.6s |
| `Hurt_Pain_01.wav` | Suara kesakitan | 0.3-0.6s |
| `Hurt_Gasp_01.wav` | Terkejut/terengah | 0.2-0.4s |

## Bedanya dengan Enemy Death?

- Hurt: saat kena damage tapi belum mati
- Death: saat HP habis
- Hurt lebih pendek dan less intense dari death

## Cara Pakai

```csharp
public AudioClip hurtSFX;

public void TakeDamage(int damage) {
    // ... kode damage ...
    
    if (!isDead && hurtSFX != null) {
        audioSource.PlayOneShot(hurtSFX);
    }
}
```
