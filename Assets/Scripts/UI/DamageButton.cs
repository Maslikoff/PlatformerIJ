public class DamageButton : ButtonHealth
{
    public override void Execute()
    {
        Health.TakeDamage(Amount);
    }
}