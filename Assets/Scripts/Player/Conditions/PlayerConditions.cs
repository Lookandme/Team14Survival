using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDamagable //데미지를 받는 물체의 인터페이스
{
     void GetDamage(float damage);
}
public class PlayerConditions : MonoBehaviour , IDamagable
{
    ConditionData conditiondata;

    public Condition health;
    public Condition stamina;
    public Condition hunger;
    public Condition thirst;

    public float starveDamage;
    public float thirstyDamage;

    public event Action PlayerOnAttack;

    private void Awake()
    {
        health = gameObject.AddComponent<Condition>();
        stamina = gameObject.AddComponent<Condition>();
        hunger = gameObject.AddComponent<Condition>();
        thirst = gameObject.AddComponent<Condition>();
    }

    private void Start()
    {
        conditiondata = CharacterManager.Instance.Player.playerData;
        PlayerDataBridge bridge = CharacterManager.Instance.Player.dataBridge;


        health.SetValue(conditiondata.health.MaxValue, conditiondata.health.passiveValue);
        stamina.SetValue(conditiondata.stamina.MaxValue, conditiondata.stamina.passiveValue);
        hunger.SetValue(conditiondata.hunger.MaxValue, conditiondata.hunger.passiveValue);
        thirst.SetValue(conditiondata.thirst.MaxValue, conditiondata.thirst.passiveValue);

        if (!bridge.isFistRun)
        {
            health.SetValue(bridge.health);
            stamina.SetValue(bridge.stamina);
            hunger.SetValue(bridge.hunger);
            thirst.SetValue(bridge.thirst);
        }
    }

    private void Update()
    {
      

        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        thirst.Subtract(thirst.passiveValue * Time.deltaTime);
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
        //Debug.Log("health:" + health.GetValue());
        //Debug.Log("stamina:" + stamina.GetValue());
        //Debug.Log("hunger:" + hunger.GetValue());
        //Debug.Log("thirst:" + thirst.GetValue());
    }

    private void Starve()
    {
        if(hunger.GetValue() <= 0.0f)
        {
            health.Subtract(starveDamage*Time.deltaTime);
        }
    }
    private void Thirsty()
    {
        if (thirst.GetValue() <= 0.0f)
        {
            stamina.Subtract(thirstyDamage*Time.deltaTime);
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

        stamina.Subtract(value);
        return true;
    }

    public void GetDamage(float damage)
    {
        health.Subtract(damage);
        PlayerOnAttack?.Invoke(); //피격 시 구독중인 메서드 호출
    }

    public List<float> SendConditionData()
    {
        List<float> data = new List<float>();
        data.Add(health.GetValue());
        data.Add(stamina.GetValue());
        data.Add(hunger.GetValue());
        data.Add(thirst.GetValue());
        
        return data;
    }
}