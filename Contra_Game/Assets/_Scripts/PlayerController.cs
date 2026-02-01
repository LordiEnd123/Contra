using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Double Jump Settings")]
    public int extraJumpsValue = 2;
    private int extraJumps;

    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;

    // Ссылка на камеру для слежения за мышкой
    private Camera mainCam;

    void Start()
    {
        // Находим камеру при старте
        mainCam = Camera.main;
    }

    void Update()
    {
        // 1. ЧИТАЕМ ДВИЖЕНИЕ
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. ЛОГИКА ПРЫЖКОВ (Твой старый код)
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
        }

        // 3. ПОВОРОТ ПЕРСОНАЖА ЗА МЫШКОЙ (НОВОЕ!) 🖱️
        if (mainCam != null)
        {
            // Переводим положение мыши из экрана в игровые координаты
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            // Если мышка справа от нас (> x), а мы смотрим влево -> Повернись
            if (mousePos.x > transform.position.x && !isFacingRight)
            {
                Flip();
            }
            // Если мышка слева от нас (< x), а мы смотрим вправо -> Повернись
            else if (mousePos.x < transform.position.x && isFacingRight)
            {
                Flip();
            }
        }
    }

    void FixedUpdate()
    {
        // Двигаем физикой
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}