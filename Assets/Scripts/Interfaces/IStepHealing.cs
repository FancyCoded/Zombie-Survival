using System.Collections;

public interface IStepHealing
{
    IEnumerator StepHealing(float healthAmount, float healStep, float healDelay);
}