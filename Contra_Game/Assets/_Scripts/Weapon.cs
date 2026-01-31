using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool isSpreadGun = false;

    // --- НОВОЕ: Переменные для звука ---
    public AudioClip shootSound; // Сюда перетащим файл звука
    private AudioSource audioSource; // Это компонент-"колонка", который играет звук

    void Start()
    {
        // --- НОВОЕ: Находим "колонку" на игроке при старте
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
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