public class HungerUI : ConditionUI
{
    protected override void Start()
    {
        base.Start();
        

    }
    protected override void Update()
    {
        SetHungertFilled();
        base.Update();
    }

    private void SetHungertFilled()
    {
        filledUIValue = data.hunger.GetPercentage();
    }
}
