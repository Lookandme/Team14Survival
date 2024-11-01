public class StaminaUI : ConditionUI
{
    protected override void Start()
    {
        base.Start();
       
    }

    protected override void Update()
    {
        SetStaminaFilled();
        base.Update();
    }

    private void SetStaminaFilled()
    {
        filledUIValue = data.stamina.GetPercentage();
    }
}
