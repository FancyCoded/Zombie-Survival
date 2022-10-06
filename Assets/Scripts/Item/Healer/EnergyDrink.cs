public class EnergyDrink : Healer
{
    public override void Heal(Player player) =>
        player.TakeHeal(Properties.HealthAmount, Properties.HealStep, Properties.HealDelay);      
}