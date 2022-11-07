public class OnceAreaDamager : BaseAreaDamager
{
    public override void Activate()
    {
        base.Activate();

        foreach (Health health in _damagedObjects)
        {
            health.TakeDamage(Damage);
        }
    }
}
