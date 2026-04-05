# SFX (Sound Effects)

Folder untuk semua sound effects dalam game.

## Subfolder

| Folder | Keterangan |
|--------|------------|
| Combat/ | SFX bertarung (hit, swing, block, death) |
| Player/ | SFX player (footstep, jump, hurt) |
| UI/ | SFX interface (click, hover, popup) |

## Tips

- Gunakan nama file yang deskriptif: `Sword_Hit_01.wav`
- Tambahkan nomor untuk variasi: `_01`, `_02`, `_03`
- Durasi ideal: 0.1 - 1 detik
- Jangan terlalu keras (bisa diatur volume di Unity)

## Cara Pakai di Script

```csharp
AudioSource.PlayOneShot(hitSFX);
```
