public class ThirstUI : ConditionUI
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SetFilled()
    {
        filledUIValue = data.thirst.GetPercentage();
    }
}