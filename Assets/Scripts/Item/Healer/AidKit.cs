public class AidKit : Healer
{
    public override void Heal(Player player) => player.TakeHeal(Properties.HealthAmount);
}