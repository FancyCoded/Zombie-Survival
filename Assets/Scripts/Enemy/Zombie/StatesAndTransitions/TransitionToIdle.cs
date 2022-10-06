public class TransitionToIdle : Transition
{
    private void Update()
    {
        if (Target.Health.IsDied)
            NeedTransit = true;
    }
}