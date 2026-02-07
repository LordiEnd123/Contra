using UnityEngine;

public class PupilController : MonoBehaviour
{
    [Header("Настройки")]
    public float eyeRadius = 0.5f; // Насколько далеко зрачок может уйти от центра
    public float moveSpeed = 5f;   // Как быстро зрачок двигается

    private Transform player;
    private Vector3 initialPosition;

    void Start()
    {
        // Запоминаем, где зрачок был в начале (это наш центр)
        initialPosition = transform.localPosition;

        // Ищем игрока
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Глаз не нашел Игрока! Проверь тег 'Player'.");
        }
    }

    void Update()
    {
        if (player == null) return;

        // 1. Вычисляем направление на игрока в локальных координатах глаза
        // Мы берем позицию игрока относительно центра нашего глаза
        Vector3 direction = player.position - transform.parent.position;

        // 2. Ограничиваем это направление нашим радиусом
        // Зрачок не сможет уйти дальше, чем eyeRadius
        Vector3 targetPosition = Vector3.ClampMagnitude(direction, eyeRadius);

        // 3. Плавно двигаем зрачок к нужной точке
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
    }

    // Рисуем круг в редакторе, чтобы видеть границы глаза
    void OnDrawGizmosSelected()
    {
        if (transform.parent != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.parent.position, eyeRadius);
        }
    }
}