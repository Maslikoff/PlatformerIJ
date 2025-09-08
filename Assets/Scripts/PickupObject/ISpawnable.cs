using System;

public interface ISpawnable
{
    event Action<ISpawnable> Spawned;
    void Despawn();
}