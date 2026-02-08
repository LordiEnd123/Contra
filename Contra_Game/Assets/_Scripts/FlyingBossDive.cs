// Assets/_Scripts/FlyingBossDive.cs
using UnityEngine;

[DisallowMultipleComponent]
public sealed class FlyingBossDive : MonoBehaviour
{
    [Header("Activation")]
    [SerializeField] private float activationDistance = 15f;

    [Header("Dive Targeting")]
    [SerializeField] private float diveHorizontalOffset = 1.2f;

    [Range(0f, 1f)]
    [SerializeField] private float aimDirectChance = 0.6f;

    [Header("Dive Homing (X only)")]
    [SerializeField] private bool homingDuringDive = true;
    [SerializeField] private float homingXSpeed = 6f;

    [Header("Heights")]
    [SerializeField] private float highYOffset = 4.5f;
    [SerializeField] private float lowYOffset = 0.6f;

    [Header("Movement Speeds")]
    [SerializeField] private float highFollowSpeed = 3f;
    [SerializeField] private float diveSpeed = 7f;
    [SerializeField] private float returnSpeed = 5f;

    [Header("Timing")]
    [SerializeField] private float highHoldSeconds = 1.6f;
    [SerializeField] private float telegraphSeconds = 0.35f;
    [SerializeField] private float vulnerableSeconds = 1.1f;

    [Header("Damage")]
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float damageCooldown = 0.8f;

    [Header("Landing Slam")]
    [SerializeField] private float slamRadius = 1.1f;
    [SerializeField] private float slamExtraRadius = 0.35f;
    [SerializeField] private bool returnUpAfterHit = true;

    [Header("Visual")]
    [Tooltip("If set, uses SpriteRenderer.flipX instead of changing transform scale (recommended).")]
    [SerializeField] private SpriteRenderer visualRenderer;

    private Transform player;
    private Rigidbody2D rb;

    private float stateTimer;
    private float lastDamageTime = -999f;

    private Vector2 diveTarget;
    private Vector2 highAnchor;

    private Vector3 baseScale;
    private Vector2 lockedLowPos;


    private enum State
    {
        Sleeping,
        HighHover,
        Telegraph,
        DiveDown,
        VulnerableLow,
        ReturnUp
    }

    private State state = State.Sleeping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (visualRenderer == null) visualRenderer = GetComponentInChildren<SpriteRenderer>();

        baseScale = transform.localScale; 
    }


    private void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        SetBossDamageEnabled(false);
    }

    private void Update()
    {
        if (player == null) return;

        var col = GetComponent<Collider2D>();
        if (col != null && !col.enabled) return;

        if (state == State.Sleeping)
        {
            if (Vector2.Distance(transform.position, player.position) <= activationDistance)
            {
                ActivateBossIfPossible();
                EnterHighHover();
            }
            return;
        }

        switch (state)
        {
            case State.HighHover:
                UpdateHighHover();
                break;
            case State.Telegraph:
                UpdateTelegraph();
                break;
            case State.DiveDown:
                UpdateDiveDown();
                break;
            case State.VulnerableLow:
                UpdateVulnerableLow();
                break;
            case State.ReturnUp:
                UpdateReturnUp();
                break;
        }

        FacePlayer();
    }

    private void EnterHighHover()
    {
        state = State.HighHover;
        stateTimer = highHoldSeconds;
        SetBossDamageEnabled(false);
    }

    private void UpdateHighHover()
    {
        highAnchor = new Vector2(player.position.x, player.position.y + highYOffset);
        MoveTowards(highAnchor, highFollowSpeed);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            state = State.Telegraph;
            stateTimer = telegraphSeconds;
            SetBossDamageEnabled(false);

            diveTarget = ComputeDiveTargetSnapshot();
        }
    }

    private void UpdateTelegraph()
    {
        MoveTowards(highAnchor, highFollowSpeed * 0.5f);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            state = State.DiveDown;
            SetBossDamageEnabled(false);
        }
    }

    private void UpdateDiveDown()
    {
        if (MoveTowards(diveTarget, diveSpeed))
        {
            Vector2 landingPos = rb != null ? rb.position : (Vector2)transform.position;

            lockedLowPos = landingPos;  
            diveTarget = landingPos;     

            TrySlamDamage(landingPos);

            state = State.VulnerableLow;
            stateTimer = vulnerableSeconds;
            SetBossDamageEnabled(true);
        }

    }

    private void UpdateVulnerableLow()
    {
        
        MoveTowards(lockedLowPos, 0f);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            state = State.ReturnUp;
            SetBossDamageEnabled(false);
            highAnchor = new Vector2(player.position.x, player.position.y + highYOffset);
        }
    }


    private void UpdateReturnUp()
    {
        if (MoveTowards(highAnchor, returnSpeed))
            EnterHighHover();
    }

    private Vector2 ComputeDiveTargetSnapshot()
    {
        float px = player.position.x;
        float py = player.position.y;

        bool aimDirect = Random.value < aimDirectChance;

        float offset = 0f;
        if (!aimDirect)
            offset = (Random.value < 0.5f ? -1f : 1f) * Mathf.Abs(diveHorizontalOffset);

        return new Vector2(px + offset, py + lowYOffset);
    }

    private void TrySlamDamage(Vector2 landingPos)
    {
        if (slamRadius <= 0f) return;
        if (Time.time - lastDamageTime < damageCooldown) return;

        float effectiveRadius = slamRadius + slamExtraRadius;
        if (Vector2.Distance(player.position, landingPos) > effectiveRadius) return;

        lastDamageTime = Time.time;

        var hp = player.GetComponent<PlayerHealth>();
        if (hp != null) hp.TakeDamage(contactDamage);

        if (returnUpAfterHit)
        {
            state = State.ReturnUp;
            SetBossDamageEnabled(false);
            highAnchor = new Vector2(player.position.x, player.position.y + highYOffset);
        }
    }

    private bool MoveTowards(Vector2 target, float speed)
    {
        Vector2 pos = rb != null ? rb.position : (Vector2)transform.position;
        Vector2 next = Vector2.MoveTowards(pos, target, speed * Time.deltaTime);

        if (rb != null) rb.MovePosition(next);
        else transform.position = next;

        return Vector2.Distance(next, target) <= 0.02f;
    }

    private void FacePlayer()
    {
        if (player == null) return;

        // Лучший вариант: флип через SpriteRenderer, не трогает scale
        if (visualRenderer != null)
        {
            visualRenderer.flipX = player.position.x > transform.position.x;
            return;
        }

        // Фолбэк: сохраняем твой размер (baseScale), меняем только знак X
        float sign = (player.position.x < transform.position.x) ? 1f : -1f;

        Vector3 s = baseScale;
        s.x = Mathf.Abs(baseScale.x) * sign;
        transform.localScale = s;
    }


    private void ActivateBossIfPossible()
    {
        gameObject.SendMessage("ForceActivate", SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessage("SetHealthBarVisible", true, SendMessageOptions.DontRequireReceiver);
    }

    private void SetBossDamageEnabled(bool enabled)
    {
        gameObject.SendMessage("SetDamageEnabled", enabled, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time - lastDamageTime < damageCooldown) return;

        lastDamageTime = Time.time;

        var hp = other.GetComponent<PlayerHealth>();
        if (hp != null) hp.TakeDamage(contactDamage);

        if (returnUpAfterHit)
        {
            state = State.ReturnUp;
            SetBossDamageEnabled(false);
            if (player != null)
                highAnchor = new Vector2(player.position.x, player.position.y + highYOffset);
        }
    }
}
