using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // ѕровер€ем, что в нас вошел именно »грок
        if (collision.tag == "Player")
        {
            // »щем у игрока скрипт Weapon
            Weapon playerWeapon = collision.GetComponent<Weapon>();

            if (playerWeapon != null)
            {
                // ¬ключаем режим дробовика
                playerWeapon.ActivateSpreadGun();

                // Ёффект подбора (вывод в консоль)
                Debug.Log("S-GUN ACTIVATED!");

                // ”дал€ем саму коробку со сцены
                Destroy(gameObject);
            }
        }
    }
}