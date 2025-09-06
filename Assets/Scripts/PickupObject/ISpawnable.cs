using System;

public interface ISpawnable
{
    event Action OnDespawn;
    void Despawn();
}
