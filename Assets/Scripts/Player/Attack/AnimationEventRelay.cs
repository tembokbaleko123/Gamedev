using UnityEngine;

// Pakai metode Bridge (relay) karena animator berada di parent bukan di child
public class AnimationEventRelay : MonoBehaviour
{
    public WeaponDamage weapon;

    public void StartDamage()
    {
        weapon.StartDamage();
    }

    public void EndDamage()
    {
        weapon.EndDamage();
    }
}