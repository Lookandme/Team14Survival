public class StaminaUI : ConditionUI
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SetStaminaFilled()
    {
        filledUIValue = data.stamina.GetPercentage();
    }
}
