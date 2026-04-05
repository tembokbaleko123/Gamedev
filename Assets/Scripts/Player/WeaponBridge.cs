using UnityEngine;

public class WeaponBridge : MonoBehaviour
{
    [SerializeField] private WeaponTrail weaponTrail;

    void Awake()
    {
        if (weaponTrail == null)
            weaponTrail = GetComponentInChildren<WeaponTrail>();
    }

    public void StartTrail()
    {
        weaponTrail?.StartTrail();
    }

    public void StopTrail()
    {
        weaponTrail?.StopTrail();
    }

    public void SpawnSlashParticle()
    {
        weaponTrail?.SpawnSlashParticle();
    }
}
