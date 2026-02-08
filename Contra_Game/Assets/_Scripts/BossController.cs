using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Boss")]
    public float maxHealth = 100f;
    public float activationDistance = 15f;
    public string nextLevelName = "Level2";

    [Header("UI")]
    public Slider healthBar;
    public GameObject winText;

    [Header("Damage Gate")]
    public bool startInvulnerable = false;

    private float currentHealth;
    private Transform player;
    private bool isBossActive;
    private bool damageEnabled = true;

    public bool IsActive => isBossActive;

    private void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            healthBar.gameObject.SetActive(false);
        }

        if (winText != null) winText.SetActive(false);
    }

    private void Update()
    {
        if (isBossActive || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < activationDistance)
            ActivateBoss();
    }

    public void ForceActivate()
    {
        if (!isBossActive) ActivateBoss();
    }

    public void SetDamageEnabled(bool enabled)
    {
        damageEnabled = enabled;
    }

    public void SetHealthBarVisible(bool visible)
    {
        if (healthBar != null) healthBar.gameObject.SetActive(visible);
    }

    private void ActivateBoss()
    {
        isBossActive = true;
        damageEnabled = !startInvulnerable;
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.value = currentHealth;
        }
        Debug.Log("Boss activated");
    }

    public void TakeDamage(int damage)
    {
        if (!isBossActive) return;
        if (!damageEnabled) return;
        currentHealth -= damage;
        if (healthBar != null) healthBar.value = currentHealth;

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log("Boss died");

        if (winText != null) winText.SetActive(true);
        if (healthBar != null) healthBar.gameObject.SetActive(false);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr != null) spr.enabled = false;

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        Invoke(nameof(LoadNextLevel), 4f);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}