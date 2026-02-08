// Assets/_Scripts/TankBossEntry.cs
using UnityEngine;

/// <summary>
/// Entry flow:
/// - when player passes activatePoint.x -> activates boss (but hides HP bar), invulnerable, no shooting
/// - drives to stopPoint.x
/// - once stopped -> shows HP bar, enables damage and shooting
/// </summary>
[DisallowMultipleComponent]
public sealed class TankBossEntry : MonoBehaviour
{
    [Header("Points")]
    public Transform activatePoint;
    public Transform stopPoint;

    [Header("Drive")]
    public float driveSpeed = 2.5f;
    public float stopEpsilon = 0.05f;

    [Header("Refs (drag from Boss_Tank)")]
    public BossController bossController;
    public EnemyShooting enemyShooting;

    private Transform player;
    private bool started;
    private bool stopped;

    private void Awake()
    {
        if (bossController == null) bossController = GetComponent<BossController>();
        if (enemyShooting == null) enemyShooting = GetComponent<EnemyShooting>();

        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x);
        transform.localScale = s;
    }

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        if (enemyShooting != null)
            enemyShooting.enabled = false;

        // На всякий случай прячем HP-bar в начале сцены
        if (bossController != null)
            bossController.SetHealthBarVisible(false);
    }

    private void Update()
    {
        if (player == null) return;
        if (activatePoint == null || stopPoint == null) return;
        if (bossController == null) return;

        if (!started)
        {
            if (player.position.x < activatePoint.position.x) return;

            started = true;

            bossController.startInvulnerable = true;
            bossController.ForceActivate();

            // Едет -> урон нельзя + HP-bar скрыт + не стреляет
            bossController.SetDamageEnabled(false);
            bossController.SetHealthBarVisible(false);

            if (enemyShooting != null)
                enemyShooting.enabled = false;

            Debug.Log("TankBossEntry started (invulnerable, no shooting, hp hidden)");
        }

        if (stopped) return;

        DriveToStopPointX();

        if (stopped)
        {
            // Встал -> урон можно + показываем HP + включаем стрельбу
            bossController.SetDamageEnabled(true);
            bossController.SetHealthBarVisible(true);

            if (enemyShooting != null)
                enemyShooting.enabled = true;

            Debug.Log("TankBossEntry finished (damage ON, shooting ON, hp shown)");
        }
    }

    private void DriveToStopPointX()
    {
        Vector3 pos = transform.position;
        float targetX = stopPoint.position.x;
        float dx = targetX - pos.x;

        if (Mathf.Abs(dx) <= stopEpsilon)
        {
            pos.x = targetX;
            transform.position = pos;
            stopped = true;
            return;
        }

        float dir = Mathf.Sign(dx);
        pos.x += dir * driveSpeed * Time.deltaTime;
        pos.x = dir > 0f ? Mathf.Min(pos.x, targetX) : Mathf.Max(pos.x, targetX);
        transform.position = pos;
    }
}
