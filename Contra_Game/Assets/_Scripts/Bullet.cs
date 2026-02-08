using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Bullet : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 10;

    [Header("Behaviour")]
    [SerializeField] private bool isLaser = false;

    [Tooltip("Layers treated as world geometry (ground/walls/camerawalls/etc).")]
    [SerializeField] private LayerMask worldLayers;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player")) return;
        if (hit.GetComponent<Bullet>() != null) return;
        if (hit.GetComponent<PowerUp>() != null) return;

        BossController boss = hit.GetComponentInParent<BossController>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            if (!isLaser) Destroy(gameObject);
            return;
        }
        Enemy enemy = hit.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            if (!isLaser) Destroy(gameObject);
            return;
        }
        if ((worldLayers.value & (1 << hit.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!isLaser)
        {
            Destroy(gameObject);
        }
    }
}
