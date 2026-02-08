using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D playerCollider;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;

    [Header("Platform Drop Settings")]
    public LayerMask platformLayer;

    [Header("Double Jump Settings")]
    public int extraJumpsValue = 2;
    private int extraJumps;

    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;

    // Переменные для приседания
    private Vector2 standSize;
    private Vector2 standOffset;
    private Vector2 crouchSize;
    private Vector2 crouchOffset;
    private bool isCrouching = false;

    // Чтобы анимация не зависала
    private string currentAnimState;

    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_RUN = "Player_Run";

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        if (animator == null)
            animator = GetComponent<Animator>();

        // Запоминаем размеры
        standSize = playerCollider.size;
        standOffset = playerCollider.offset;
        crouchSize = new Vector2(standSize.x, standSize.y / 2f);
        crouchOffset = new Vector2(standOffset.x, standOffset.y - (standSize.y / 4f));
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            ChangeAnimation(PLAYER_RUN);
        }
        else
        {
            ChangeAnimation(PLAYER_IDLE);
        }

        // Поворот
        if (horizontalInput > 0 && !isFacingRight) Flip();
        else if (horizontalInput < 0 && isFacingRight) Flip();

        // Прыжок вниз
        if (Input.GetKey(KeyCode.S) && Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3f, platformLayer);
            if (hit.collider != null)
            {
                StartCoroutine(FallThrough(hit.collider));
            }
        }

        // Приседание
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

        // Прыжок вверх
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
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void ChangeAnimation(string newState)
    {
        // Если эта анимация уже играет - выходим
        if (currentAnimState == newState) return;

        // Запускаем анимацию
        animator.Play(newState);

        // Запоминаем, что сейчас играет
        currentAnimState = newState;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator FallThrough(Collider2D platformCollider)
    {
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}