public class HealButton : ButtonHealth
{
    public override void Execute()
    {
        Health.Heal(Amount);
    }
}