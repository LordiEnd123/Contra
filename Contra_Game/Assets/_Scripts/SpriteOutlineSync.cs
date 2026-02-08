using UnityEngine;

[ExecuteAlways]
[DisallowMultipleComponent]
public sealed class SpriteOutlineSync : MonoBehaviour
{
    [SerializeField] private SpriteRenderer source;
    [SerializeField] private SpriteRenderer target;
    [SerializeField] private int orderOffset = -1;

    public SpriteRenderer Source
    {
        get => source;
        set => source = value;
    }

    public SpriteRenderer Target
    {
        get => target;
        set => target = value;
    }

    public int OrderOffset
    {
        get => orderOffset;
        set => orderOffset = value;
    }

    private void Reset()
    {
        target = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (source == null || target == null) return;

        target.enabled = source.enabled;
        target.sprite = source.sprite;
        target.flipX = source.flipX;
        target.flipY = source.flipY;

        target.sortingLayerID = source.sortingLayerID;
        target.sortingOrder = source.sortingOrder + orderOffset;
    }
}
