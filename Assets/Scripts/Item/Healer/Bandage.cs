public class Bandage : Healer
{
    public override void Heal(Player player) => player.TakeHeal(Properties.HealthAmount);
}