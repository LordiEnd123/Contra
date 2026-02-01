using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D playerCollider; // Твой коллайдер
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Platform Drop Settings")]
    public LayerMask platformLayer; // ВЫБЕРИ ЗДЕСЬ СЛОЙ "Passable"

    [Header("Double Jump Settings")]
    public int extraJumpsValue = 2;
    private int extraJumps;

    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;

    // Переменные для приседания (размеры)
    private Vector2 standSize;
    private Vector2 standOffset;
    private Vector2 crouchSize;
    private Vector2 crouchOffset;
    private bool isCrouching = false;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        // Запоминаем размеры
        standSize = playerCollider.size;
        standOffset = playerCollider.offset;
        crouchSize = new Vector2(standSize.x, standSize.y / 2f);
        crouchOffset = new Vector2(standOffset.x, standOffset.y - (standSize.y / 4f));
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // --- 1. ПРЫЖОК ВНИЗ (ИСПРАВЛЕНО) ---
        if (Input.GetKey(KeyCode.S) && Input.GetButtonDown("Jump"))
        {
            // РИСУЕМ ЛАЗЕР, чтобы ты видел его в Scene (Красная линия)
            Debug.DrawRay(groundCheck.position, Vector2.down * 3f, Color.red, 2f);

            // Увеличили длину до 3f, чтобы точно достать!
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3f, platformLayer);

            if (hit.collider != null)
            {
                Debug.Log("ВИЖУ ПЛАТФОРМУ: " + hit.collider.name); // Проверка в консоль
                StartCoroutine(FallThrough(hit.collider));
            }
            else
            {
                Debug.Log("НЕ ВИЖУ ПЛАТФОРМУ! Луч промахнулся.");
            }
        }

        // --- 2. ПРИСЕДАНИЕ ---
        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            playerCollider.size = crouchSize;
            playerCollider.offset = crouchOffset;
            isCrouching = true;
        }
        else
        {
            if (isCrouching)
            {
                playerCollider.size = standSize;
                playerCollider.offset = standOffset;
                isCrouching = false;
            }
        }

        // --- 3. ПРЫЖОК ВВЕРХ ---
        if (isGrounded) extraJumps = extraJumpsValue;

        if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.S) && extraJumps > 0)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.S) && extraJumps == 0 && isGrounded)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
        }

        // --- 4. ПОВОРОТ ---
        if (mainCam != null)
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x > transform.position.x && !isFacingRight) Flip();
            else if (mousePos.x < transform.position.x && isFacingRight) Flip();
        }
    }

    void FixedUpdate()
    {
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

    // --- ГЛАВНАЯ МАГИЯ ---
    IEnumerator FallThrough(Collider2D platformCollider)
    {
        // Игнорируем столкновение ТОЛЬКО между Игроком и ЭТОЙ платформой
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);

        // Ждем 0.5 секунды
        yield return new WaitForSeconds(0.5f);

        // Включаем обратно
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}