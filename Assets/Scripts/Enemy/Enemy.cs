using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemySprite))]
public class Enemy : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    private EnemySprite _enemySprite;

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemySprite = GetComponent<EnemySprite>();
    }

}