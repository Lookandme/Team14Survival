using UnityEngine;
using UnityEngine.UI;

public class PlayerCoditions : MonoBehaviour
{
    //PlayerData playerdata

    public Condition health;
    public Condition stamina;
    public Condition hunger;
    public Condition thirst;

    private void Awake()
    {
        //playerdata = ChrarcterManager.instance.player.playerdata
        //health.SetValue(playerdata.hp)
        //stamina.SetValue(playerdata.st)
        //hunger.SetValue(playerdata.hg)
        //thirst.SetValue(playerdata.th)
    }

    private void Update()
    {
        hunger.Substract(hunger.passiveValue * Time.deltaTime);
        thirst.Substract(thirst.passiveValue * Time.deltaTime);
        if (thirst.GetValue() > 0.0f)
        {
            stamina.Add(stamina.passiveValue * Time.deltaTime);
        }

        Starve();
        Thirsty();

        if(health.GetValue() <= 0.0f)
        {
            Die();
        }
    }

    private void Starve()
    {
        if(hunger.GetValue() <= 0.0f)
        {
            health.Substract(health.passiveValue);
        }
    }
    private void Thirsty()
    {
        if (thirst.GetValue() <= 0.0f)
        {
            stamina.Substract(stamina.passiveValue);
        }
    }

    private void Die()
    {
        //추후 구현 논의
    }

    public void Heal(float value)
    {
        health.Add(value);
    }

    public void Eat(float value)
    {
        hunger.Add(value);
    }

    public void Drink(float value)
    {
        thirst.Add(value);
    }

    public bool StaminaConsumption(float value)
    {
        if (stamina.GetValue() - value < 0.0f)
        {
            return false; 
            //지침 패널티 부여 고민중
        }

        stamina.Substract(value);
        return true;
    }
}