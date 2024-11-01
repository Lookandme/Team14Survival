public class ThirstUI : ConditionUI
{
    protected override void Start()
    {
        base.Start();
       
    }

    protected override void Update()
    {
        SetThirstFilled();
        base.Update();
    }

    private void SetThirstFilled()
    {
        filledUIValue = data.thirst.GetPercentage();
    }
}