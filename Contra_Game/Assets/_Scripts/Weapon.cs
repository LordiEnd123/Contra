using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool isSpreadGun = false;

    // --- НОВОЕ: Переменные для звука ---
    public AudioClip shootSound; // Сюда перетащим файл звука
    private AudioSource audioSource; // Это компонент-"колонка", который играет звук

    // Ссылка на камеру, чтобы найти мышку
    private Camera mainCam;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        // 1. ПОВОРОТ ЗА МЫШКОЙ 🖱️
        // Переводим позицию мыши из пикселей экрана в координаты игрового мира
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Вычисляем направление от пушки к мыши
        Vector3 direction = mousePos - transform.position;

        // Вычисляем угол поворота в градусах (магия тригонометрии)
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Применяем поворот
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        if (rotZ > 90 || rotZ < -90)
        {
            // Переворачиваем пушку вверх ногами по Y, чтобы она выглядела нормально
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            // Возвращаем как было
            transform.localScale = new Vector3(1, 1, 1);
        }

        // 2. СТРЕЛЬБА
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // --- НОВОЕ: Если звук есть и колонка есть — ИГРАЕМ!
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound); // PlayOneShot позволяет накладывать звуки друг на друга
        }

        // Старая логика стрельбы...
        if (isSpreadGun)
        {
            CreateBullet(0);
            CreateBullet(15);
            CreateBullet(-15);
        }
        else
        {
            CreateBullet(0);
        }
    }

    void CreateBullet(float angleOffset)
    {
        Quaternion rotation = firePoint.rotation;
        rotation *= Quaternion.Euler(0, 0, angleOffset);
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, rotation);

        if (transform.localScale.x < 0)
        {
            newBullet.transform.Rotate(0f, 180f, 0f);
        }
    }

    public void ActivateSpreadGun()
    {
        isSpreadGun = true;
    }
}