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

    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;

    void Update()
    {
        // 1. ������ ������� (A/D ��� �������)
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Raw ���� ������ ����� ��� �������

        // 2. ������ (������ ���� �� �����)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 3. ������� ������� (����)
        if (horizontalInput > 0 && !isFacingRight) Flip();
        else if (horizontalInput < 0 && isFacingRight) Flip();
    }

    void FixedUpdate()
    {
        // 4. ���������� �������� (������ � FixedUpdate!)
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 5. �������� ����� (������ ��������� ��������� ���� ��� ������)
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