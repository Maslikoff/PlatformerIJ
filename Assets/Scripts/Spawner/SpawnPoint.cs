using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [field: SerializeField] public Transform Point { get; private set; }
    public bool IsOccupied { get; set; }
}