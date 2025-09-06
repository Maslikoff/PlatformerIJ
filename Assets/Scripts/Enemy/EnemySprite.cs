using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemySprite : MonoBehaviour
{
    [SerializeField] private bool _flipBasedOnDirection = true;
    [SerializeField] private float _directionThreshold = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private EnemyMovement _enemyMovement;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (_flipBasedOnDirection && _enemyMovement != null)
            UpdateSpriteFlip();
    }

    private void UpdateSpriteFlip()
    {
        Vector2 direction = _enemyMovement.MovementDirection;

        if (direction.x > _directionThreshold)
            _spriteRenderer.flipX = false;
        else if (direction.x < -_directionThreshold)
            _spriteRenderer.flipX = true;
    }
}
