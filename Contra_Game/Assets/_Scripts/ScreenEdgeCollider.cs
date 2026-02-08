using UnityEngine;

[ExecuteInEditMode] // Чтобы работало даже не нажимая Play
public class ScreenEdgeCollider : MonoBehaviour
{
    private Camera cam;
    private BoxCollider2D col;

    void Start()
    {
        cam = Camera.main; // Находим камеру
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (cam == null) return;

        // 1. Считаем высоту и ширину экрана в игровых единицах
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // 2. Двигаем стену на левый край
        // Ширина / 2 — это расстояние от центра до края.
        // Ставим минус, чтобы было слева.
        // Добавляем 0.5f (половина ширины стены), чтобы она была чуть-чуть за кадром
        transform.localPosition = new Vector3(-(screenWidth / 2) - 0.5f, 0, 0);

        // 3. Растягиваем коллайдер по высоте, чтобы он всегда закрывал экран сверху донизу
        if (col != null)
        {
            col.size = new Vector2(1f, screenHeight + 2f); // +2 про запас
        }
    }
}