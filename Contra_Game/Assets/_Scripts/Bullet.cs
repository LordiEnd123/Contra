using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    void Start() // ����������� ��� �������� ����
    {
        // ����� ����, ���� ������� ���� (������)
        rb.linearVelocity = transform.right * speed;

        // ������� ���� ����� 3 �������, ���� ��� ������ �� ������
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // ���� ����� ������� ���� �����
        Debug.Log("����� �: " + hitInfo.name);

        // ���������� ���� ��� �����
        Destroy(gameObject);
    }
}