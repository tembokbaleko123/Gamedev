# Audio Folder

Folder ini berisi semua audio untuk game Hack and Slash.

## Struktur Folder

```
Audio/
├── SFX/           # Sound Effects (suara pendek, event-based)
├── BGM/           # Background Music (musik latar)
└── Ambience/      # Suara environment (angin, api, dll)
```

## Format File yang Diterima

| Format | Rekomendasi |
|--------|-------------|
| .wav   | ⭐ SFX (best quality) |
| .mp3   | BGM (compressed, smaller size) |
| .ogg   | Alternative untuk BGM |

## Setting Import Unity

**Untuk SFX:**
- Load In Background: false
- Preload Audio Data: true
- Compression Format: PCM atau ADPCM

**Untuk BGM:**
- Load In Background: true
- Preload Audio Data: false
- Compression Format: Vorbis

## Catatan

- Pastikan audio tidak copyrighted (pakai royalty-free atau buat sendiri)
- Untuk SFX, durasi sebaiknya pendek (0.1 - 2 detik)
- Volume di-mix di Unity, bukan di file audio
