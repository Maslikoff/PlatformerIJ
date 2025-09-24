using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private bool _checkObstacles = true;

    public bool HasTargets => Targets.Count > 0;
    public List<Transform> Targets { get; private set; } = new List<Transform>();
    public Transform ClosestTarget { get; private set; }
    public float DistanceToClosestTarget { get; private set; }

    public void DetectTargets()
    {
       Targets.Clear();
        ClosestTarget = null;
        DistanceToClosestTarget = float.MaxValue;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _targetLayer);

        foreach (Collider2D hit in hits)
        {
            if(hit.transform == transform)
                continue;

            if(_checkObstacles && HasObstacle(hit.transform))
                continue;

            Targets.Add(hit.transform);

            float distance = Vector2.Distance(transform.position, hit.transform.position);

            if (distance < DistanceToClosestTarget)
            {
                DistanceToClosestTarget = distance;
                ClosestTarget = hit.transform;
            }
        }
    }

    public Transform GetClosestTarget()
    {
        DetectTargets();
        return ClosestTarget;
    }

    public Transform GetClosestTarget(Vector2 fromPosition)
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;

        foreach (var target in Targets)
        {
            if (target == null) continue;

            float distance = Vector2.Distance(fromPosition, target.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    private bool HasObstacle(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, _obstacleLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = HasTargets ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        if (ClosestTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, ClosestTarget.position);
        }
    }
}