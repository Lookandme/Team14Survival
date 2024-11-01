public class HealthUI : ConditionUI
{
    protected override void Start()
    {
        base.Start();
      
    }

    protected override void Update()
    {
        SetHealthFilled();
        base.Update();
    }

    private void SetHealthFilled()
    {
        filledUIValue = data.health.GetPercentage();
    }
}
