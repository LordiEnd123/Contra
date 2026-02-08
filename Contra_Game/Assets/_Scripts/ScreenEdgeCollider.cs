using UnityEngine;

[ExecuteInEditMode]
public class ScreenEdgeCollider : MonoBehaviour
{
    private Camera cam;
    private BoxCollider2D col;

    void Start()
    {
        cam = Camera.main;
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (cam == null) return;

        // 1. Считаем высоту и ширину экрана в игровых единицах
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // 2. Двигаем стену на левый край
        transform.localPosition = new Vector3(-(screenWidth / 2) - 0.5f, 0, 0);

        // 3. Растягиваем коллайдер по высоте, чтобы он всегда закрывал экран сверху донизу
        if (col != null)
        {
            col.size = new Vector2(1f, screenHeight + 2f);
        }
    }
}