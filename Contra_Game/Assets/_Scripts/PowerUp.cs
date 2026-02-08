using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum WeaponType { SpreadGun, MachineGun, LaserGun, FireGun }
    public WeaponType weaponType;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Weapon playerWeapon = collision.GetComponentInChildren<Weapon>();

            if (playerWeapon != null)
            {
                if (weaponType == WeaponType.SpreadGun) playerWeapon.ActivateSpreadGun();
                else if (weaponType == WeaponType.MachineGun) playerWeapon.ActivateMachineGun();
                else if (weaponType == WeaponType.LaserGun) playerWeapon.ActivateLaserGun();
                else if (weaponType == WeaponType.FireGun)
                {
                    playerWeapon.ActivateFireGun();
                    Debug.Log("Подобран F (Fireball)!");
                }

                Destroy(gameObject);
            }
        }
    }
}