using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _obstacleLayer;

    public bool IsPlayerDetected { get; private set; }
    public Vector2 PlayerPosition { get; private set; }
    public float DistanceToPlayer { get; private set; }

    public void DetectPlayer()
    {
        IsPlayerDetected = false;

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _detectionRadius, _playerLayer);

        if (playerCollider != null)
        {
            Vector2 directionToPlayer = (playerCollider.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, playerCollider.transform.position);

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                directionToPlayer,
                distance,
                _obstacleLayer
            );

            if (hit.collider == null)
            {
                IsPlayerDetected = true;
                PlayerPosition = playerCollider.transform.position;
                DistanceToPlayer = distance;
            }
        }
    }
}