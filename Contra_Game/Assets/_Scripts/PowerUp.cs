using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что в нас вошел именно Игрок
        if (collision.tag == "Player")
        {
            // Ищем компонент Weapon на самом объекте ИЛИ на любых его дочерних объектах
            Weapon playerWeapon = collision.GetComponentInChildren<Weapon>();

            if (playerWeapon != null)
            {
                // Включаем режим дробовика
                playerWeapon.ActivateSpreadGun();

                // Эффект подбора (вывод в консоль)
                Debug.Log("S-GUN ACTIVATED!");

                // Удаляем саму коробку со сцены
                Destroy(gameObject);
            }
        }
    }
}