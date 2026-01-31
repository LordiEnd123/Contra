using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; // Скорость бега

    void Update()
    {
        // Всегда двигаться влево (Vector2.left = x: -1, y: 0)
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    // Если враг упал в пропасть (ниже Y = -10), удаляем его, чтобы не грузить память
    void FixedUpdate()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}