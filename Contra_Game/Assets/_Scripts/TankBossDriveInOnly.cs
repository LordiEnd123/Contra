using UnityEngine;

/// <summary>
/// Drives boss tank to stopPoint on X once BossController activates.
/// Keeps existing EnemyShooting for firing.
/// </summary>
[DisallowMultipleComponent]
public sealed class TankBossDriveInOnly : MonoBehaviour
{
    public Transform stopPoint;
    public float driveSpeed = 2.5f;
    public float stopEpsilon = 0.05f;

    private BossController bossController;
    private bool stopped;

    private void Awake()
    {
        bossController = GetComponent<BossController>();

        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x); // face right
        transform.localScale = s;
    }

    private void Update()
    {
        if (stopped) return;
        if (stopPoint == null) return;
        if (bossController == null || !bossController.IsActive) return;

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

        if (dir > 0f) pos.x = Mathf.Min(pos.x, targetX);
        else pos.x = Mathf.Max(pos.x, targetX);

        transform.position = pos;
    }
}
