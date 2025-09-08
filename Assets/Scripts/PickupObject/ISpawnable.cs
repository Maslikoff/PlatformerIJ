using System;

public interface ISpawnable
{
    event Action<ISpawnable> OnDespawn;
    void Despawn();
}
