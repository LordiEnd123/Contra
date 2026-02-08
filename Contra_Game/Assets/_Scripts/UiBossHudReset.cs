using UnityEngine;

public sealed class UiBossHudReset : MonoBehaviour
{
    [Header("Disable these objects on scene load")]
    public GameObject bossHpSlider;
    public GameObject winBossText;

    private void Awake()
    {
        if (bossHpSlider != null) bossHpSlider.SetActive(false);
        if (winBossText != null) winBossText.SetActive(false);
    }
}
