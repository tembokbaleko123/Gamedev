# Ambience

Suara environment/latar belakang arena.

## File yang Direkomendasikan

| Nama File | Keterangan |
|-----------|------------|
| `Arena_Wind.wav` | Suara angin |
| `Arena_Fire.wav` | Api/jamur (jika arena ada api) |
| `Arena_Crowd.wav` | Suara penonton (jika arena gladiator) |
| `Arena_Cave.wav` | Reverb cave (jika arena dalam gua) |

## Cara Pakai

Ambience di-play secara looping di background:

```csharp
public class AmbienceManager : MonoBehaviour {
    public AudioClip ambienceClip;
    private AudioSource audioSource;
    
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ambienceClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
```

## Setting AudioSource

- Spatial Blend: 0 (2D, tidak berubah by distance)
- Loop: true
- Volume: 0.2 - 0.4 (jangan terlalu keras, background only)

## Catatan

Ambience ditambahkan nanti saat Hari 6-7 saat arena sudah dibuat.
Saat ini bisa dikosongkan dulu.
